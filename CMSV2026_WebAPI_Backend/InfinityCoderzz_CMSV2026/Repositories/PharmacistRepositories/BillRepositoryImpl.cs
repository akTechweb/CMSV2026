using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories
{
    public class BillRepositoryImpl : IPharmacyBillRepository
    {
        private readonly string _connectionString;

        public BillRepositoryImpl(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStr")!;
        }

        #region Get Patients

        public async Task<IEnumerable<PatientLookup>> GetPatients()
        {
            List<PatientLookup> patients = new();
            await using SqlConnection conn = new(_connectionString);
            await using SqlCommand cmd = new("sp_GetPatients", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                patients.Add(new PatientLookup
                {
                    PatientId   = Convert.ToInt32(reader["PatientId"]),
                    PatientCode = reader["PatientCode"]?.ToString(),
                    FullName    = reader["FullName"]?.ToString()
                });
            }
            return patients;
        }

        #endregion

        #region Get Medicines For Billing

        public async Task<IEnumerable<MedicineLookup>> GetMedicinesForBilling()
        {
            List<MedicineLookup> medicines = new();
            await using SqlConnection conn = new(_connectionString);
            await using SqlCommand cmd = new("sp_GetMedicinesForBilling", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                medicines.Add(new MedicineLookup
                {
                    MedicineId   = Convert.ToInt32(reader["MedicineId"]),
                    MedicineCode = reader["MedicineCode"]?.ToString(),
                    MedicineName = reader["MedicineName"]?.ToString(),
                    UnitPrice    = Convert.ToDecimal(reader["UnitPrice"])
                });
            }
            return medicines;
        }

        #endregion

        #region Get All Bills

        public async Task<IEnumerable<BillViewModel>> GetAllBills()
        {
            List<BillViewModel> bills = new();
            await using SqlConnection conn = new(_connectionString);
            await using SqlCommand cmd = new("sp_GetPharmacyBills", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                bills.Add(new BillViewModel
                {
                    BillId      = Convert.ToInt32(reader["BillId"]),
                    PatientId   = Convert.ToInt32(reader["PatientId"]),
                    BillDate    = Convert.ToDateTime(reader["BillDate"]),
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                    Status      = reader["Status"]?.ToString(),
                    PatientName = reader["PatientName"]?.ToString(),
                    PatientCode = reader["PatientCode"]?.ToString()
                });
            }
            return bills;
        }

        #endregion

        #region Get Bill By Id

        public async Task<BillViewModel?> GetBillById(int billId)
        {
            await using SqlConnection conn = new(_connectionString);
            await using SqlCommand cmd = new("sp_GetPharmacyBillById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillId", billId);

            await conn.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new BillViewModel
                {
                    BillId      = Convert.ToInt32(reader["BillId"]),
                    PatientId   = Convert.ToInt32(reader["PatientId"]),
                    BillDate    = Convert.ToDateTime(reader["BillDate"]),
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                    Status      = reader["Status"]?.ToString(),
                    PatientName = reader["PatientName"]?.ToString(),
                    PatientCode = reader["PatientCode"]?.ToString()
                };
            }
            return null;
        }

        #endregion

        #region Get Bill Items

        public async Task<IEnumerable<BillItemViewModel>> GetBillItems(int billId)
        {
            List<BillItemViewModel> items = new();
            await using SqlConnection conn = new(_connectionString);
            await using SqlCommand cmd = new("sp_GetPharmacyBillItemsByBill", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillId", billId);

            await conn.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                items.Add(new BillItemViewModel
                {
                    BillItemId   = Convert.ToInt32(reader["BillItemId"]),
                    BillId       = Convert.ToInt32(reader["BillId"]),
                    MedicineId   = Convert.ToInt32(reader["MedicineId"]),
                    MedicineName = reader["MedicineName"]?.ToString(),
                    Quantity     = Convert.ToInt32(reader["Quantity"]),
                    UnitPrice    = Convert.ToDecimal(reader["UnitPrice"]),
                    Amount       = Convert.ToDecimal(reader["Amount"])
                });
            }
            return items;
        }

        #endregion

        #region Get Bill → Prescription Link

        public async Task<BillPrescriptionLink?> GetBillPrescriptionLink(int billId)
        {
            await using SqlConnection conn = new(_connectionString);
            await using SqlCommand cmd = new("sp_GetPrescriptionLinkByBill", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillId", billId);

            await conn.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new BillPrescriptionLink
                {
                    PrescriptionId     = Convert.ToInt32(reader["PrescriptionId"]),
                    DispenseId         = Convert.ToInt32(reader["DispenseId"]),
                    PrescriptionStatus = reader["PrescriptionStatus"]?.ToString()
                };
            }
            return null;
        }

        #endregion

        #region Create Bill

        public async Task<int> CreateBill(CreateBillViewModel model, int staffId)
        {
            await using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();
            using SqlTransaction tx = conn.BeginTransaction();

            try
            {
                SqlCommand billCmd = new("sp_CreatePharmacyBillHeader", conn, tx);
                billCmd.CommandType = CommandType.StoredProcedure;
                billCmd.Parameters.AddWithValue("@PatientId",          model.PatientId);
                billCmd.Parameters.AddWithValue("@TotalAmount",         model.TotalAmount);
                billCmd.Parameters.AddWithValue("@GeneratedByStaffId", staffId);

                SqlParameter billIdParam = new("@BillId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                billCmd.Parameters.Add(billIdParam);
                await billCmd.ExecuteNonQueryAsync();

                int billId = Convert.ToInt32(billIdParam.Value);

                foreach (var item in model.BillItems)
                {
                    SqlCommand itemCmd = new("sp_AddPharmacyBillItem", conn, tx);
                    itemCmd.CommandType = CommandType.StoredProcedure;
                    itemCmd.Parameters.AddWithValue("@BillId",       billId);
                    itemCmd.Parameters.AddWithValue("@MedicineId",   item.MedicineId);
                    itemCmd.Parameters.AddWithValue("@MedicineName", item.MedicineName ?? string.Empty);
                    itemCmd.Parameters.AddWithValue("@Quantity",     item.Quantity);
                    itemCmd.Parameters.AddWithValue("@UnitPrice",    item.UnitPrice);
                    itemCmd.Parameters.AddWithValue("@Amount",       item.Amount);
                    await itemCmd.ExecuteNonQueryAsync();
                }

                tx.Commit();
                return billId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        #endregion

        #region Cancel Bill

        public async Task CancelBill(int billId, int staffId, string? reason)
        {
            // ── Pre-check 1: Verify the bill exists ───────────────────────────
            var bill = await GetBillById(billId);
            if (bill == null)
                throw new KeyNotFoundException($"Bill {billId} not found.");

            // ── Pre-check 2: Guard against double-cancellation ────────────────
            if (string.Equals(bill.Status, "Cancelled", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Bill {billId} is already cancelled.");

            // ── Proceed: delegate stock restoration to the stored procedure ───
            // NOTE: sp_CancelPharmacyBill restores stock by MedicineId + Quantity.
            // Because PharmacyBillItems does not store StockId, exact FEFO batch
            // restoration is not possible without a schema change. The SP restores
            // to the available batch it finds — this is an accepted schema limitation.
            await using SqlConnection conn = new(_connectionString);
            await using SqlCommand cmd = new("sp_CancelPharmacyBill", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillId",  billId);
            cmd.Parameters.AddWithValue("@StaffId", staffId);
            cmd.Parameters.AddWithValue("@Reason",  (object?)reason ?? DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        #endregion
    }
}
