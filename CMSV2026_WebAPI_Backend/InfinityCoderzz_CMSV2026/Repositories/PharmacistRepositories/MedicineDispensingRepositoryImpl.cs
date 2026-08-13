using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories
{
    public class MedicineDispensingRepositoryImpl : IMedicineDispensingRepository
    {
        private readonly string _connectionString;

        public MedicineDispensingRepositoryImpl(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStr")!;
        }

        // ---- Prescriptions that can still be dispensed ----------------------
        public async Task<IEnumerable<Prescription>> GetDispensablePrescriptions()
        {
            List<Prescription> list = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetAllPrescriptions", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                string? status = reader["Status"]?.ToString();
                if (!string.IsNullOrEmpty(status) &&
                    (status.Equals("Dispensed",  StringComparison.OrdinalIgnoreCase) ||
                     status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase)))
                    continue;

                list.Add(new Prescription
                {
                    PrescriptionId   = Convert.ToInt32(reader["PrescriptionId"]),
                    PatientId        = reader["PatientId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PatientId"]),
                    PatientName      = reader["PatientName"]?.ToString(),
                    DoctorId         = reader["DoctorId"]  == DBNull.Value ? 0 : Convert.ToInt32(reader["DoctorId"]),
                    DoctorName       = reader["DoctorName"]?.ToString(),
                    PrescriptionDate = reader["PrescriptionDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["PrescriptionDate"]),
                    Remarks          = reader["Remarks"]?.ToString(),
                    Status           = string.IsNullOrEmpty(status) ? "Pending" : status
                });
            }

            return list;
        }

        // ---- Dispense + auto-generate bill (single transaction) -------------
        public async Task<DispenseBillResult> DispenseAndBill(int prescriptionId, int staffId, string? remarks)
        {
            List<(int MedicineId, int Quantity)> items = await GetItemsToDispenseAsync(prescriptionId);
            if (items.Count == 0)
                throw new InvalidOperationException("This prescription has no medicines to dispense.");

            int patientId = await GetPrescriptionPatientIdAsync(prescriptionId);
            if (patientId <= 0)
                throw new InvalidOperationException("Could not resolve the patient for this prescription.");

            await using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();
            using SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                // 1. dispensing header → DispenseId
                int dispenseId;
                using (SqlCommand createCmd = new("sp_CreateMedicineDispensing", connection, transaction))
                {
                    createCmd.CommandType = CommandType.StoredProcedure;
                    createCmd.Parameters.AddWithValue("@PrescriptionId",     prescriptionId);
                    createCmd.Parameters.AddWithValue("@DispensedByStaffId", staffId);
                    createCmd.Parameters.AddWithValue("@Remarks",            (object?)remarks ?? DBNull.Value);
                    object? scalar = await createCmd.ExecuteScalarAsync();
                    dispenseId = scalar == null ? 0 : Convert.ToInt32(scalar);
                }

                if (dispenseId <= 0)
                    throw new InvalidOperationException("Failed to create the dispensing record.");

                // 2. FEFO stock deduction per medicine
                foreach (var (medicineId, quantity) in items)
                {
                    using SqlCommand dispCmd = new("sp_DispenseMedicine", connection, transaction);
                    dispCmd.CommandType = CommandType.StoredProcedure;
                    dispCmd.Parameters.AddWithValue("@DispenseId",         dispenseId);
                    dispCmd.Parameters.AddWithValue("@MedicineId",         medicineId);
                    dispCmd.Parameters.AddWithValue("@QuantityDispensed",  quantity);
                    await dispCmd.ExecuteNonQueryAsync();
                }

                // 3. read back priced dispense lines to mirror in the bill
                List<(int MedicineId, string MedicineName, int Quantity, decimal UnitPrice, decimal Amount)> billLines = new();
                decimal total = 0m;
                using (SqlCommand readCmd = new("sp_GetDispensingItems", connection, transaction))
                {
                    readCmd.CommandType = CommandType.StoredProcedure;
                    readCmd.Parameters.AddWithValue("@DispenseId", dispenseId);
                    using SqlDataReader reader = (SqlDataReader)await readCmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int     medId   = Convert.ToInt32(reader["MedicineId"]);
                        string  medName = reader["MedicineName"]?.ToString() ?? string.Empty;
                        int     qty     = Convert.ToInt32(reader["QuantityDispensed"]);
                        decimal price   = Convert.ToDecimal(reader["UnitPrice"]);
                        decimal amount  = Convert.ToDecimal(reader["Amount"]);
                        billLines.Add((medId, medName, qty, price, amount));
                        total += amount;
                    }
                }

                // 4. bill header → BillId
                int billId;
                using (SqlCommand billCmd = new("sp_CreatePharmacyBillHeader", connection, transaction))
                {
                    billCmd.CommandType = CommandType.StoredProcedure;
                    billCmd.Parameters.AddWithValue("@PatientId",          patientId);
                    billCmd.Parameters.AddWithValue("@TotalAmount",         total);
                    billCmd.Parameters.AddWithValue("@GeneratedByStaffId", staffId);
                    SqlParameter billIdParam = new("@BillId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    billCmd.Parameters.Add(billIdParam);
                    await billCmd.ExecuteNonQueryAsync();
                    billId = Convert.ToInt32(billIdParam.Value);
                }

                // 5. bill items — no second stock deduction
                foreach (var line in billLines)
                {
                    using SqlCommand itemCmd = new("sp_AddPharmacyBillItemNoStock", connection, transaction);
                    itemCmd.CommandType = CommandType.StoredProcedure;
                    itemCmd.Parameters.AddWithValue("@BillId",       billId);
                    itemCmd.Parameters.AddWithValue("@MedicineId",   line.MedicineId);
                    itemCmd.Parameters.AddWithValue("@MedicineName", line.MedicineName);
                    itemCmd.Parameters.AddWithValue("@Quantity",     line.Quantity);
                    itemCmd.Parameters.AddWithValue("@UnitPrice",    line.UnitPrice);
                    itemCmd.Parameters.AddWithValue("@Amount",       line.Amount);
                    await itemCmd.ExecuteNonQueryAsync();
                }

                // 6. link bill ↔ prescription ↔ dispense
                using (SqlCommand linkCmd = new("sp_LinkBillToPrescription", connection, transaction))
                {
                    linkCmd.CommandType = CommandType.StoredProcedure;
                    linkCmd.Parameters.AddWithValue("@BillId",         billId);
                    linkCmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
                    linkCmd.Parameters.AddWithValue("@DispenseId",     dispenseId);
                    await linkCmd.ExecuteNonQueryAsync();
                }

                // 7. mark prescription as dispensed
                using (SqlCommand statusCmd = new("sp_UpdatePrescriptionStatus", connection, transaction))
                {
                    statusCmd.CommandType = CommandType.StoredProcedure;
                    statusCmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
                    statusCmd.Parameters.AddWithValue("@Status",         "Dispensed");
                    await statusCmd.ExecuteNonQueryAsync();
                }

                // 8. audit log
                using (SqlCommand auditCmd = new("sp_AddAuditLog", connection, transaction))
                {
                    auditCmd.CommandType = CommandType.StoredProcedure;
                    auditCmd.Parameters.AddWithValue("@StaffId", staffId);
                    auditCmd.Parameters.AddWithValue("@Action",  "Dispense & Bill");
                    auditCmd.Parameters.AddWithValue("@Remarks",
                        $"Dispensed prescription #{prescriptionId} ({billLines.Count} item(s)) and generated bill #{billId}.");
                    await auditCmd.ExecuteNonQueryAsync();
                }

                transaction.Commit();
                return new DispenseBillResult { DispenseId = dispenseId, BillId = billId };
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // ---- History -------------------------------------------------------
        public async Task<IEnumerable<DispensingHistoryViewModel>> GetDispensingHistory()
        {
            List<DispensingHistoryViewModel> list = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetDispensingHistory", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new DispensingHistoryViewModel
                {
                    DispenseId     = Convert.ToInt32(reader["DispenseId"]),
                    PrescriptionId = Convert.ToInt32(reader["PrescriptionId"]),
                    PatientName    = reader["PatientName"]?.ToString(),
                    PharmacistName = reader["PharmacistName"]?.ToString(),
                    DispenseDate   = Convert.ToDateTime(reader["DispenseDate"]),
                    Remarks        = reader["Remarks"]?.ToString(),
                    TotalItems     = Convert.ToInt32(reader["TotalItems"]),
                    TotalAmount    = reader["TotalAmount"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["TotalAmount"])
                });
            }

            return list;
        }

        // ---- Items for one dispensing --------------------------------------
        public async Task<IEnumerable<MedicineDispensingItem>> GetDispensingItems(int dispenseId)
        {
            List<MedicineDispensingItem> items = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetDispensingItems", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DispenseId", dispenseId);

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                items.Add(new MedicineDispensingItem
                {
                    DispenseItemId    = Convert.ToInt32(reader["DispenseItemId"]),
                    DispenseId        = Convert.ToInt32(reader["DispenseId"]),
                    MedicineId        = Convert.ToInt32(reader["MedicineId"]),
                    MedicineName      = reader["MedicineName"]?.ToString(),
                    QuantityDispensed = Convert.ToInt32(reader["QuantityDispensed"]),
                    UnitPrice         = Convert.ToDecimal(reader["UnitPrice"]),
                    Amount            = Convert.ToDecimal(reader["Amount"])
                });
            }

            return items;
        }

        // ---- Private helpers -----------------------------------------------
        private async Task<int> GetPrescriptionPatientIdAsync(int prescriptionId)
        {
            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetPrescriptionById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                return reader["PatientId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PatientId"]);

            return 0;
        }

        private async Task<List<(int MedicineId, int Quantity)>> GetItemsToDispenseAsync(int prescriptionId)
        {
            List<(int, int)> items = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetPrescriptionItems", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                int medicineId = Convert.ToInt32(reader["MedicineId"]);
                int quantity   = reader["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Quantity"]);
                if (quantity > 0)
                    items.Add((medicineId, quantity));
            }

            return items;
        }


        // ---- Pre-dispense stock check ---------------------------------------
        public async Task<StockCheckResult> CheckStock(int prescriptionId)
        {
            StockCheckResult result = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_CheckPrescriptionStock", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Items.Add(new StockCheckItem
                {
                    MedicineId = Convert.ToInt32(reader["MedicineId"]),
                    MedicineName = reader["MedicineName"]?.ToString() ?? string.Empty,
                    Required = Convert.ToInt32(reader["Required"]),
                    Available = Convert.ToInt32(reader["Available"])
                });
            }

            result.CanDispense = result.Items.Count > 0 && result.Items.All(i => !i.IsShort);
            return result;
        }
    }
}
