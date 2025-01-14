using API.Net.Models;
using API.Net.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Net.Controllers


{
    [Route("users")]  // Route cơ bản cho controller
    public class UserController : Controller
    {
        private readonly DbtestContext _context;

        public UserController(DbtestContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách sản phẩm



        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();
            // Lấy toàn bộ sản phẩm
            return Ok(users);

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Lấy một sản phẩm theo ID
            var user = _context.Users
                .Include(p => p.Orders)
         .FirstOrDefault(p => p.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserModel dto)
        {
            if (string.IsNullOrEmpty(dto.FullName))
            {
                return StatusCode(402, new { message = "Đã xảy ra lỗi trong quá trình xử lý.", error = "Null rồi" });
            }

            string hashedPassword = HashPassword(dto.Password); // code mới nè

            User user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                //Password = dto.Password,
                Password = hashedPassword, // Lưu mật khẩu đã mã hóa
                CreateAt = DateTime.UtcNow,
                RoleId = 1
            };

            try
            {
                // Lưu vào cơ sở dữ liệu
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Đăng ký thành công.",
                    user = new
                    {
                        dto.Email,
                        dto.FullName,
                        user.UserId,
                        user.CreateAt
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình xử lý.", error = ex.Message });
            }
        }

        private string HashPassword(string password)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("X2")); 
                }

                return sb.ToString();
            }
        }


            [HttpGet("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new { message = "Email và mật khẩu không được để trống." });
            }
            string hashedPassword = HashPassword(password); // Mã hóa mật khẩu nhập vào
            try
            {

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword);

                if (user == null)
                {
                    return StatusCode(404, new { message = "Khong tim thay user." });

                }

                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình xử lý.", error = ex.Message });
            }
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(int userId, string currentPassword, string newPassword, string confirmNewPassword)
        {
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
            {
                return BadRequest(new { message = "Mật khẩu không được để trống." });
            }

            if (newPassword != confirmNewPassword)
            {
                return BadRequest(new { message = "Mật khẩu mới và xác nhận mật khẩu không khớp." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng." });
            }

            // Validate the current password
            string hashedCurrentPassword = HashPassword(currentPassword);
            if (user.Password != hashedCurrentPassword)
            {
                return Unauthorized(new { message = "Mật khẩu hiện tại không đúng." });
            }

            // Hash the new password
            string hashedNewPassword = HashPassword(newPassword);
            user.Password = hashedNewPassword;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Đổi mật khẩu thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình xử lý.", error = ex.Message });
            }
        }

    }

}
