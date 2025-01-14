using API.Net.Models;
using Microsoft.AspNetCore.Mvc;
using Web.net.Service;

namespace Web.net.Controllers
{
    public class SignUpController : Controller
    {
        private readonly UserService userService;

        public SignUpController(UserService userService)
        {

            this.userService = userService;

        }
        public async Task<IActionResult> Register(string Email, string Password, int PhoneNumber, string FullName)
        {
            // Tạo object user với dữ liệu cần gửi
            var user = new
            {
                FullName = FullName,
                Email = Email,
                Password = Password,
                PhoneNumber = PhoneNumber
            };

            // Gọi hàm CreateUserAsync với object user
            var result = await userService.CreateUserAsync(user);

            if (result)
            {
                // Thành công, chuyển hướng đến trang chủ
                return RedirectToAction("Index", "Home");
            }

            // Nếu thất bại, hiển thị lại View hiện tại
            return View();
        }


        public IActionResult Index()
        {



            return View();
        }
    }
}
