namespace InfinityCoderzz_CMSV2026.Models.pharmacist
{
    /// <summary>Stock availability for one prescribed medicine.</summary>
    public class StockCheckItem
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int Required { get; set; }
        public int Available { get; set; }
        public bool IsShort => Available < Required;
    }

    /// <summary>Result of a pre-dispense stock check for a whole prescription.</summary>
    public class StockCheckResult
    {
        public bool CanDispense { get; set; }
        public List<StockCheckItem> Items { get; set; } = new();
    }
}
