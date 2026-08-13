using System.ComponentModel.DataAnnotations;

namespace InfinityCoderzz_CMSV2026.DTOs.Pharmacy
{
    /// <summary>Request body for PUT api/pharmacist/prescriptions/{id}/status.</summary>
    public class UpdatePrescriptionStatusRequest
    {
        [Required(ErrorMessage = "Status cannot be empty.")]
        public string Status { get; set; } = string.Empty;
    }
}
