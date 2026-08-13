using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories
{
    public class MedicineRepositoryImpl : IMedicineRepository
    {
        private readonly string _connectionString;

        public MedicineRepositoryImpl(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStr")!;
        }

        #region Get All Medicines

        public async Task<IEnumerable<Medicine>> GetAllMedicines()
        {
            List<Medicine> medicines = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand cmd = new("sp_GetAllMedicines", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                medicines.Add(new Medicine
                {
                    MedicineId       = Convert.ToInt32(reader["MedicineId"]),
                    MedicineCode     = reader["MedicineCode"]?.ToString(),
                    MedicineName     = reader["MedicineName"]?.ToString(),
                    GenericName      = reader["GenericName"]?.ToString(),
                    CategoryId       = Convert.ToInt32(reader["CategoryId"]),
                    CategoryName     = reader["CategoryName"]?.ToString(),
                    ManufacturerId   = Convert.ToInt32(reader["ManufacturerId"]),
                    ManufacturerName = reader["ManufacturerName"]?.ToString(),
                    Unit             = reader["Unit"]?.ToString(),
                    UnitPrice        = Convert.ToDecimal(reader["UnitPrice"]),
                    ReorderLevel     = Convert.ToInt32(reader["ReorderLevel"]),
                    IsActive         = Convert.ToBoolean(reader["IsActive"])
                });
            }

            return medicines;
        }

        #endregion

        #region Search Medicine

        public async Task<IEnumerable<Medicine>> SearchMedicine(string searchTerm)
        {
            List<Medicine> medicines = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand cmd = new("sp_SearchMedicine", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                medicines.Add(new Medicine
                {
                    MedicineId       = Convert.ToInt32(reader["MedicineId"]),
                    MedicineCode     = reader["MedicineCode"]?.ToString(),
                    MedicineName     = reader["MedicineName"]?.ToString(),
                    GenericName      = reader["GenericName"]?.ToString(),
                    CategoryName     = reader["CategoryName"]?.ToString(),
                    ManufacturerName = reader["ManufacturerName"]?.ToString(),
                    Unit             = reader["Unit"]?.ToString(),
                    UnitPrice        = Convert.ToDecimal(reader["UnitPrice"]),
                    ReorderLevel     = Convert.ToInt32(reader["ReorderLevel"]),
                    IsActive         = Convert.ToBoolean(reader["IsActive"])
                });
            }

            return medicines;
        }

        #endregion

        #region Get Medicine By Id

        public async Task<Medicine?> GetMedicineById(int medicineId)
        {
            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand cmd = new("sp_GetMedicineById", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MedicineId", medicineId);

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Medicine
                {
                    MedicineId     = Convert.ToInt32(reader["MedicineId"]),
                    MedicineCode   = reader["MedicineCode"]?.ToString(),
                    MedicineName   = reader["MedicineName"]?.ToString(),
                    GenericName    = reader["GenericName"]?.ToString(),
                    CategoryId     = Convert.ToInt32(reader["CategoryId"]),
                    ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]),
                    Unit           = reader["Unit"]?.ToString(),
                    UnitPrice      = Convert.ToDecimal(reader["UnitPrice"]),
                    ReorderLevel   = Convert.ToInt32(reader["ReorderLevel"]),
                    IsActive       = Convert.ToBoolean(reader["IsActive"])
                };
            }

            return null;
        }

        #endregion

        #region Add Medicine

        public async Task AddMedicine(Medicine medicine)
        {
            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand cmd = new("sp_InsertMedicine", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MedicineCode",   medicine.MedicineCode);
            cmd.Parameters.AddWithValue("@MedicineName",   medicine.MedicineName);
            cmd.Parameters.AddWithValue("@GenericName",    medicine.GenericName ?? string.Empty);
            cmd.Parameters.AddWithValue("@CategoryId",     medicine.CategoryId);
            cmd.Parameters.AddWithValue("@ManufacturerId", medicine.ManufacturerId);
            cmd.Parameters.AddWithValue("@Unit",           medicine.Unit ?? string.Empty);
            cmd.Parameters.AddWithValue("@UnitPrice",      medicine.UnitPrice);
            cmd.Parameters.AddWithValue("@ReorderLevel",   medicine.ReorderLevel);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region Update Medicine

        public async Task UpdateMedicine(Medicine medicine)
        {
            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand cmd = new("sp_UpdateMedicine", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MedicineId",     medicine.MedicineId);
            cmd.Parameters.AddWithValue("@MedicineCode",   medicine.MedicineCode);
            cmd.Parameters.AddWithValue("@MedicineName",   medicine.MedicineName);
            cmd.Parameters.AddWithValue("@GenericName",    medicine.GenericName ?? string.Empty);
            cmd.Parameters.AddWithValue("@CategoryId",     medicine.CategoryId);
            cmd.Parameters.AddWithValue("@ManufacturerId", medicine.ManufacturerId);
            cmd.Parameters.AddWithValue("@Unit",           medicine.Unit ?? string.Empty);
            cmd.Parameters.AddWithValue("@UnitPrice",      medicine.UnitPrice);
            cmd.Parameters.AddWithValue("@ReorderLevel",   medicine.ReorderLevel);
            cmd.Parameters.AddWithValue("@IsActive",       medicine.IsActive);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region Disable Medicine

        public async Task DisableMedicine(int medicineId)
        {
            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand cmd = new("sp_DisableMedicine", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MedicineId", medicineId);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region Get All Categories

        public async Task<IEnumerable<MedicineCategory>> GetAllCategories()
        {
            List<MedicineCategory> categories = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand cmd = new("sp_GetAllMedicineCategories", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(new MedicineCategory
                {
                    CategoryId   = Convert.ToInt32(reader["CategoryId"]),
                    CategoryName = reader["CategoryName"]?.ToString()
                });
            }

            return categories;
        }

        #endregion

        #region Get All Manufacturers

        public async Task<IEnumerable<Manufacturer>> GetAllManufacturers()
        {
            List<Manufacturer> manufacturers = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand cmd = new("sp_GetAllManufacturers", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                manufacturers.Add(new Manufacturer
                {
                    ManufacturerId   = Convert.ToInt32(reader["ManufacturerId"]),
                    ManufacturerName = reader["ManufacturerName"]?.ToString()
                });
            }

            return manufacturers;
        }

        #endregion
    }
}
