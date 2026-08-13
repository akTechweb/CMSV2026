using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories
{
    public class InventoryLogRepositoryImpl : IInventoryLogRepository
    {
        private readonly string _connectionString;

        public InventoryLogRepositoryImpl(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStr")!;
        }

        public async Task<IEnumerable<MedicineInventoryLog>> GetInventoryLogs()
        {
            List<MedicineInventoryLog> logs = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetInventoryLogs", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                logs.Add(new MedicineInventoryLog
                {
                    InventoryLogId  = Convert.ToInt32(reader["InventoryLogId"]),
                    MedicineName    = reader["MedicineName"]?.ToString(),
                    QuantityChanged = Convert.ToInt32(reader["QuantityChanged"]),
                    TransactionType = reader["TransactionType"]?.ToString(),
                    TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                    Remarks         = reader["Remarks"]?.ToString()
                });
            }

            return logs;
        }
    }
}
