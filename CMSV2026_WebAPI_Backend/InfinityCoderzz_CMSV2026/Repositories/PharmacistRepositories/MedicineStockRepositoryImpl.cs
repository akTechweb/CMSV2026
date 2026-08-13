using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories
{
    public class MedicineStockRepositoryImpl : IMedicineStockRepository
    {
        private readonly string _connectionString;

        public MedicineStockRepositoryImpl(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStr")!;
        }

        #region Get All Medicine Stock

        public async Task<IEnumerable<MedicineStock>> GetAllMedicineStock()
        {
            List<MedicineStock> stocks = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetAllMedicineStock", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                stocks.Add(new MedicineStock
                {
                    StockId       = Convert.ToInt32(reader["StockId"]),
                    MedicineId    = Convert.ToInt32(reader["MedicineId"]),
                    MedicineName  = reader["MedicineName"]?.ToString(),
                    BatchNumber   = reader["BatchNumber"]?.ToString(),
                    Quantity      = Convert.ToInt32(reader["Quantity"]),
                    PurchasePrice = reader["PurchasePrice"] == DBNull.Value ? null : Convert.ToDecimal(reader["PurchasePrice"]),
                    ExpiryDate    = Convert.ToDateTime(reader["ExpiryDate"]),
                    PurchaseDate  = reader["PurchaseDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["PurchaseDate"]),
                    CreatedAt     = Convert.ToDateTime(reader["CreatedAt"])
                });
            }

            return stocks;
        }

        #endregion

        #region Get Stock By Id

        public async Task<MedicineStock?> GetMedicineStockById(int stockId)
        {
            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetMedicineStockById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@StockId", stockId);

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new MedicineStock
                {
                    StockId       = Convert.ToInt32(reader["StockId"]),
                    MedicineId    = Convert.ToInt32(reader["MedicineId"]),
                    MedicineName  = reader["MedicineName"]?.ToString(),
                    BatchNumber   = reader["BatchNumber"]?.ToString(),
                    Quantity      = Convert.ToInt32(reader["Quantity"]),
                    PurchasePrice = reader["PurchasePrice"] == DBNull.Value ? null : Convert.ToDecimal(reader["PurchasePrice"]),
                    ExpiryDate    = Convert.ToDateTime(reader["ExpiryDate"]),
                    PurchaseDate  = reader["PurchaseDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["PurchaseDate"]),
                    CreatedAt     = Convert.ToDateTime(reader["CreatedAt"])
                };
            }

            return null;
        }

        #endregion

        #region Add Stock

        public async Task AddMedicineStock(MedicineStock stock)
        {
            await using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();

            await using SqlCommand command = new("sp_AddMedicineStock", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MedicineId",    stock.MedicineId);
            command.Parameters.AddWithValue("@BatchNumber",   stock.BatchNumber);
            command.Parameters.AddWithValue("@Quantity",      stock.Quantity);
            command.Parameters.AddWithValue("@PurchasePrice", stock.PurchasePrice ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ExpiryDate",    stock.ExpiryDate);
            command.Parameters.AddWithValue("@PurchaseDate",  stock.PurchaseDate ?? (object)DBNull.Value);
            await command.ExecuteNonQueryAsync();

            await using SqlCommand logCommand = new("sp_AddInventoryLog", connection);
            logCommand.CommandType = CommandType.StoredProcedure;
            logCommand.Parameters.AddWithValue("@MedicineId",       stock.MedicineId);
            logCommand.Parameters.AddWithValue("@QuantityChanged",  stock.Quantity);
            logCommand.Parameters.AddWithValue("@TransactionType",  "Stock Added");
            logCommand.Parameters.AddWithValue("@Remarks",          "New stock batch added");
            await logCommand.ExecuteNonQueryAsync();
        }

        #endregion

        #region Update Stock

        public async Task UpdateMedicineStock(MedicineStock stock)
        {
            await using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();

            await using SqlCommand command = new("sp_UpdateMedicineStock", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@StockId",       stock.StockId);
            command.Parameters.AddWithValue("@Quantity",      stock.Quantity);
            command.Parameters.AddWithValue("@PurchasePrice", stock.PurchasePrice ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ExpiryDate",    stock.ExpiryDate);
            await command.ExecuteNonQueryAsync();

            await using SqlCommand logCommand = new("sp_AddInventoryLog", connection);
            logCommand.CommandType = CommandType.StoredProcedure;
            logCommand.Parameters.AddWithValue("@MedicineId",       stock.MedicineId);
            logCommand.Parameters.AddWithValue("@QuantityChanged",  stock.Quantity);
            logCommand.Parameters.AddWithValue("@TransactionType",  "Stock Updated");
            logCommand.Parameters.AddWithValue("@Remarks",          "Stock details updated");
            await logCommand.ExecuteNonQueryAsync();
        }

        #endregion

        #region Low Stock

        public async Task<IEnumerable<MedicineStock>> GetLowStockMedicines()
        {
            List<MedicineStock> stocks = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetLowStockMedicines", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            HashSet<string> columns = new(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < reader.FieldCount; i++)
                columns.Add(reader.GetName(i));

            while (await reader.ReadAsync())
            {
                MedicineStock item = new()
                {
                    MedicineId   = Convert.ToInt32(reader["MedicineId"]),
                    MedicineName = reader["MedicineName"]?.ToString(),
                    Quantity     = Convert.ToInt32(reader["Quantity"])
                };

                if (columns.Contains("StockId") && reader["StockId"] != DBNull.Value)
                    item.StockId = Convert.ToInt32(reader["StockId"]);

                if (columns.Contains("BatchNumber") && reader["BatchNumber"] != DBNull.Value)
                    item.BatchNumber = reader["BatchNumber"].ToString();

                if (columns.Contains("ExpiryDate") && reader["ExpiryDate"] != DBNull.Value)
                    item.ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"]);

                if (columns.Contains("DaysRemaining") && reader["DaysRemaining"] != DBNull.Value)
                    item.DaysRemaining = Convert.ToInt32(reader["DaysRemaining"]);

                stocks.Add(item);
            }

            return stocks;
        }

        #endregion

        #region Expiring Medicines

        public async Task<IEnumerable<MedicineStock>> GetExpiringMedicines()
        {
            List<MedicineStock> stocks = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetExpiringMedicines", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                stocks.Add(new MedicineStock
                {
                    StockId      = Convert.ToInt32(reader["StockId"]),
                    MedicineName = reader["MedicineName"]?.ToString(),
                    BatchNumber  = reader["BatchNumber"]?.ToString(),
                    Quantity     = Convert.ToInt32(reader["Quantity"]),
                    ExpiryDate   = Convert.ToDateTime(reader["ExpiryDate"]),
                    DaysRemaining = Convert.ToInt32(reader["DaysRemaining"])
                });
            }

            return stocks;
        }

        #endregion

        #region Expired Medicines

        public async Task<IEnumerable<MedicineStock>> GetExpiredMedicines()
        {
            List<MedicineStock> stocks = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetExpiredMedicines", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                stocks.Add(new MedicineStock
                {
                    StockId      = Convert.ToInt32(reader["StockId"]),
                    MedicineName = reader["MedicineName"]?.ToString(),
                    BatchNumber  = reader["BatchNumber"]?.ToString(),
                    Quantity     = Convert.ToInt32(reader["Quantity"]),
                    ExpiryDate   = Convert.ToDateTime(reader["ExpiryDate"])
                });
            }

            return stocks;
        }

        #endregion

        #region Medicine Dropdown

        public async Task<IEnumerable<Medicine>> GetAllMedicines()
        {
            List<Medicine> medicines = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_GetActiveMedicinesForDropdown", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                medicines.Add(new Medicine
                {
                    MedicineId   = Convert.ToInt32(reader["MedicineId"]),
                    MedicineName = reader["MedicineName"]?.ToString()
                });
            }

            return medicines;
        }

        #endregion

        #region Batch Exists

        public async Task<bool> BatchExists(int medicineId, string batchNumber, int excludeStockId)
        {
            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_CheckStockBatchExists", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MedicineId",     medicineId);
            command.Parameters.AddWithValue("@BatchNumber",    (object?)batchNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@ExcludeStockId", excludeStockId);

            await connection.OpenAsync();
            object? result = await command.ExecuteScalarAsync();
            return result != null && result != DBNull.Value && Convert.ToInt32(result) == 1;
        }

        #endregion
    }
}
