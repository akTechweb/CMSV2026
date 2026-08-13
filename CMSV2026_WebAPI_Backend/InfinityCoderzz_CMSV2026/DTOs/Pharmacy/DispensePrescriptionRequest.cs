using System.ComponentModel.DataAnnotations;

namespace InfinityCoderzz_CMSV2026.DTOs.Pharmacy
{
    /// <summary>Request body for POST api/pharmacist/dispensing.</summary>
    public class DispensePrescriptionRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A valid prescription must be selected.")]
        public int PrescriptionId { get; set; }

        public string? Remarks { get; set; }
    }
}
