using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories
{
    public class PrescriptionRepositoryImpl : IPrescriptionRepository
    {
        private readonly string _connectionString;

        public PrescriptionRepositoryImpl(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStr")!;
        }

        public async Task<IEnumerable<Prescription>> GetAllPrescriptions()
        {
            List<Prescription> prescriptions = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetAllPrescriptions", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                prescriptions.Add(new Prescription
                {
                    PrescriptionId   = Convert.ToInt32(reader["PrescriptionId"]),
                    PatientId        = Convert.ToInt32(reader["PatientId"]),
                    PatientName      = reader["PatientName"]?.ToString(),
                    DoctorId         = Convert.ToInt32(reader["DoctorId"]),
                    DoctorName       = reader["DoctorName"]?.ToString(),
                    PrescriptionDate = Convert.ToDateTime(reader["PrescriptionDate"]),
                    Remarks          = reader["Remarks"]?.ToString(),
                    Status           = reader["Status"]?.ToString()
                });
            }

            return prescriptions;
        }

        public async Task<Prescription?> GetPrescriptionById(int prescriptionId)
        {
            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetPrescriptionById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Prescription
                {
                    PrescriptionId   = Convert.ToInt32(reader["PrescriptionId"]),
                    PatientId        = Convert.ToInt32(reader["PatientId"]),
                    PatientName      = reader["PatientName"]?.ToString(),
                    DoctorId         = Convert.ToInt32(reader["DoctorId"]),
                    DoctorName       = reader["DoctorName"]?.ToString(),
                    PrescriptionDate = Convert.ToDateTime(reader["PrescriptionDate"]),
                    Remarks          = reader["Remarks"]?.ToString(),
                    Status           = reader["Status"]?.ToString()
                };
            }

            return null;
        }

        public async Task<IEnumerable<PrescriptionItem>> GetPrescriptionItems(int prescriptionId)
        {
            List<PrescriptionItem> items = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetPrescriptionItems", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                items.Add(new PrescriptionItem
                {
                    PrescriptionItemId = Convert.ToInt32(reader["PrescriptionItemId"]),
                    PrescriptionId     = Convert.ToInt32(reader["PrescriptionId"]),
                    MedicineId         = Convert.ToInt32(reader["MedicineId"]),
                    MedicineName       = reader["MedicineName"]?.ToString(),
                    Dosage             = reader["Dosage"]?.ToString(),
                    Frequency          = reader["Frequency"]?.ToString(),
                    Duration           = reader["Duration"]?.ToString(),
                    Quantity           = Convert.ToInt32(reader["Quantity"]),
                    Instructions       = reader["Instructions"]?.ToString()
                });
            }

            return items;
        }

        public async Task UpdatePrescriptionStatus(int prescriptionId, string status)
        {
            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_UpdatePrescriptionStatus", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
            command.Parameters.AddWithValue("@Status",         status);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
