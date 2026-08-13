using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        // POST: api/login
        // On success, sets the session cookie and returns the redirect target
        // (which controller/dashboard the client should call next), mirroring
        // the role-based redirects of the original MVC LoginController.
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Please enter username and password." });
            }

            string passwordHash = ComputeMd5Hash(request.Password);
            string connStr = _configuration.GetConnectionString("DefaultConnection")!;

            await using SqlConnection con = new SqlConnection(connStr);
            await using SqlCommand cmd = new SqlCommand(@"
                SELECT 
                    u.UserId,
                    u.Username,
                    r.RoleId,
                    r.RoleName,
                    s.StaffId,
                    s.FullName,
                    d.DoctorId
                FROM dbo.Users u
                INNER JOIN dbo.Roles r ON u.RoleId = r.RoleId
                LEFT JOIN dbo.Staff s ON u.UserId = s.UserId
                LEFT JOIN dbo.Doctors d ON s.StaffId = d.StaffId
                WHERE u.Username = @Username
                  AND u.PasswordHash = @PasswordHash
                  AND u.IsActive = 1", con);

            cmd.Parameters.AddWithValue("@Username", request.Username);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

            await con.OpenAsync();
            await using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            int userId = Convert.ToInt32(reader["UserId"]);
            int roleId = Convert.ToInt32(reader["RoleId"]);
            string roleName = reader["RoleName"].ToString() ?? "";
            string fullName = reader["FullName"] == DBNull.Value ? request.Username : reader["FullName"].ToString() ?? request.Username;

            HttpContext.Session.SetInt32("UserId", userId);
            HttpContext.Session.SetInt32("RoleId", roleId);
            HttpContext.Session.SetString("UserName", request.Username);
            HttpContext.Session.SetString("RoleName", roleName);
            HttpContext.Session.SetString("FullName", fullName);

            string redirectTo = roleName switch
            {
                "Doctor" => "api/doctor/dashboard",
                "Receptionist" => "api/receptionists/dashboard",
                "Pharmacist" => "api/pharmacist/dashboard",
                "Lab Technician" => "api/labtechnician/dashboard",
                "Admin" => "api/admin/dashboard",
                _ => ""
            };

            if (roleName == "Doctor" && reader["DoctorId"] != DBNull.Value)
            {
                HttpContext.Session.SetInt32("DoctorId", Convert.ToInt32(reader["DoctorId"]));
            }

            if (roleName == "Pharmacist")
            {
                int pharmacistStaffId = reader["StaffId"] == DBNull.Value ? userId : Convert.ToInt32(reader["StaffId"]);
                HttpContext.Session.SetInt32("PharmacistId", pharmacistStaffId);
                HttpContext.Session.SetString("PharmacistName", fullName);
            }

            if (string.IsNullOrEmpty(redirectTo))
            {
                return BadRequest(new { message = "Role is not configured." });
            }

            return Ok(new
            {
                userId,
                roleName,
                fullName,
                next = redirectTo
            });
        }

        // POST: api/login/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out." });
        }

        private static string ComputeMd5Hash(string input)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
