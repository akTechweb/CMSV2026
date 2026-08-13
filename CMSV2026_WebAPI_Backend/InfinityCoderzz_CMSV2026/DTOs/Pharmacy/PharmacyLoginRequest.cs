using System.ComponentModel.DataAnnotations;

namespace InfinityCoderzz_CMSV2026.DTOs.Pharmacy
{
    /// <summary>Request body for POST api/pharmacist/auth/login.</summary>
    public class PharmacyLoginRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}
