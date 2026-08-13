using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using InfinityCoderzz_CMSV2026.DTOs.Pharmacy;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzzz_CMSV2026.Controllers.Pharmacy
{
    /// <summary>
    /// Pharmacist-specific login/logout.
    /// Uses the dedicated sp_PharmacistLogin stored procedure via IPharmacyLoginService,
    /// mirroring the authentication behaviour of the original MVC PharmacistAuthorize filter.
    /// </summary>
    [ApiController]
    [Route("api/pharmacist/auth")]
    public class PharmacyLoginController : ControllerBase
    {
        private readonly IPharmacyLoginService _loginService;

        public PharmacyLoginController(IPharmacyLoginService loginService)
        {
            _loginService = loginService;
        }

        // POST: api/pharmacist/auth/login
        /// <summary>
        /// Authenticates a pharmacist and writes PharmacistId / PharmacistName into the session.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] PharmacyLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string passwordHash = ComputeMd5Hash(request.Password);
            var (staffId, fullName) = await _loginService.Login(request.Username, passwordHash);

            if (staffId <= 0)
                return Unauthorized(new { message = "Invalid username or password." });

            HttpContext.Session.SetInt32("PharmacistId",  staffId);
            HttpContext.Session.SetString("PharmacistName", fullName);

            return Ok(new
            {
                message  = "Login successful.",
                staffId,
                fullName,
                next     = "api/pharmacist/dashboard"
            });
        }

        // POST: api/pharmacist/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("PharmacistId");
            HttpContext.Session.Remove("PharmacistName");
            return Ok(new { message = "Logged out successfully." });
        }

        private static string ComputeMd5Hash(string input)
        {
            byte[] bytes = MD5.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
