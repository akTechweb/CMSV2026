namespace InfinityCoderzz_CMSV2026.DTOs.Pharmacy
{
    /// <summary>Request body for POST api/pharmacist/bills/{id}/cancel.</summary>
    public class CancelBillRequest
    {
        public string? Reason { get; set; }
    }
}
