using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories
{
    public class PharmacyLoginRepositoryImpl : IPharmacyLoginRepository
    {
        private readonly string _connectionString;

        public PharmacyLoginRepositoryImpl(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStr")!;
        }

        public async Task<(int StaffId, string FullName)> Login(string username, string passwordHash)
        {
            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new("sp_PharmacistLogin", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Username",     username);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);

            await connection.OpenAsync();
            await using SqlDataReader reader = (SqlDataReader)await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                int    staffId  = Convert.ToInt32(reader["StaffId"]);
                string fullName = reader["FullName"]?.ToString() ?? string.Empty;
                return (staffId, fullName);
            }

            return (0, string.Empty);
        }
    }
}
