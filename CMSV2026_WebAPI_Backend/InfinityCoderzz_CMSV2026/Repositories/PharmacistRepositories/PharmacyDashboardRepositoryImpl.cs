using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories
{
    public class PharmacyDashboardRepositoryImpl : IPharmacyDashboardRepository
    {
        private readonly string _connectionString;

        public PharmacyDashboardRepositoryImpl(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStr")!;
        }

        public async Task<PharmacyDashboard> GetDashboardData()
        {
            PharmacyDashboard dashboard = new();

            // Core counts via SP
            try
            {
                await using SqlConnection conn = new(_connectionString);
                await using SqlCommand cmd = new("sp_GetPharmacyDashboard", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                await conn.OpenAsync();
                await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    dashboard.TotalMedicines    = Convert.ToInt32(reader["TotalMedicines"]);
                    dashboard.TotalStockBatches = Convert.ToInt32(reader["TotalStockBatches"]);
                    dashboard.LowStockMedicines = Convert.ToInt32(reader["LowStockMedicines"]);
                    dashboard.ExpiringMedicines = Convert.ToInt32(reader["ExpiringMedicines"]);
                    dashboard.ExpiredMedicines  = Convert.ToInt32(reader["ExpiredMedicines"]);

                    if (HasColumn(reader, "PendingPrescriptions"))
                        dashboard.PendingPrescriptions = Convert.ToInt32(reader["PendingPrescriptions"]);

                    if (HasColumn(reader, "TodaysBills"))
                        dashboard.TodaysBills = Convert.ToInt32(reader["TodaysBills"]);

                    if (HasColumn(reader, "TodaysRevenue"))
                        dashboard.TodaysRevenue = reader["TodaysRevenue"] == DBNull.Value
                            ? 0m : Convert.ToDecimal(reader["TodaysRevenue"]);

                    if (HasColumn(reader, "AvailableMedicines"))
                        dashboard.AvailableMedicines = Convert.ToInt32(reader["AvailableMedicines"]);

                    if (HasColumn(reader, "ReorderRequired"))
                        dashboard.ReorderRequired = Convert.ToInt32(reader["ReorderRequired"]);

                    if (HasColumn(reader, "TodaysDispensed"))
                        dashboard.TodaysDispensed = Convert.ToInt32(reader["TodaysDispensed"]);

                    if (HasColumn(reader, "MonthlyRevenue"))
                        dashboard.MonthlyRevenue = reader["MonthlyRevenue"] == DBNull.Value
                            ? 0m : Convert.ToDecimal(reader["MonthlyRevenue"]);
                }
            }
            catch { /* SP not yet available — dashboard shows zeros */ }

            dashboard.RevenueChart    = await LoadChartAsync("sp_GetPharmacyRevenueChart");
            dashboard.DispensingChart = await LoadChartAsync("sp_GetPharmacyDispensingChart");

            // Low stock list
            try
            {
                await using SqlConnection conn2 = new(_connectionString);
                await using SqlCommand cmd2 = new("sp_GetLowStockList", conn2);
                cmd2.CommandType = CommandType.StoredProcedure;
                await conn2.OpenAsync();
                await using SqlDataReader r2 = (SqlDataReader)await cmd2.ExecuteReaderAsync();
                while (await r2.ReadAsync())
                    dashboard.LowStockList.Add(MapMedicineStock(r2));
            }
            catch { /* SP not yet available */ }

            // Expiring list
            try
            {
                await using SqlConnection conn3 = new(_connectionString);
                await using SqlCommand cmd3 = new("sp_GetExpiringList", conn3);
                cmd3.CommandType = CommandType.StoredProcedure;
                await conn3.OpenAsync();
                await using SqlDataReader r3 = (SqlDataReader)await cmd3.ExecuteReaderAsync();
                while (await r3.ReadAsync())
                    dashboard.ExpiringList.Add(MapMedicineStock(r3));
            }
            catch { /* SP not yet available */ }

            return dashboard;
        }

        private async Task<List<ChartPoint>> LoadChartAsync(string procedure)
        {
            List<ChartPoint> points = new();
            try
            {
                await using SqlConnection conn = new(_connectionString);
                await using SqlCommand cmd = new(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                await conn.OpenAsync();
                await using SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    points.Add(new ChartPoint
                    {
                        Label = reader["Label"]?.ToString() ?? string.Empty,
                        Value = reader["Value"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Value"])
                    });
                }
            }
            catch { /* SP not yet available */ }
            return points;
        }

        private static MedicineStock MapMedicineStock(SqlDataReader reader) => new()
        {
            StockId       = Convert.ToInt32(reader["StockId"]),
            MedicineId    = Convert.ToInt32(reader["MedicineId"]),
            MedicineName  = reader["MedicineName"]?.ToString(),
            BatchNumber   = reader["BatchNumber"]?.ToString() ?? string.Empty,
            Quantity      = Convert.ToInt32(reader["Quantity"]),
            ExpiryDate    = Convert.ToDateTime(reader["ExpiryDate"]),
            DaysRemaining = reader["DaysRemaining"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DaysRemaining"])
        };

        private static bool HasColumn(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }
    }
}
