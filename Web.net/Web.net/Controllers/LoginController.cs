using API.Net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Web.net.Service;

namespace Web.net.Controllers
{
    public class LoginController : Controller
    {

        private readonly UserService userService;


        public LoginController(UserService userService)
        {

            this.userService = userService;

        }
        public async Task<IActionResult>  SignIn(string email, string password)
        {

          var user =  await userService.GetLogin(email, password);

            if (user!=null)
            {
                HttpContext.Session.SetString("FullName", user.FullName);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetObject("user", user);

                return RedirectToAction("Index", "Home");
            }


            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile()
        {
            // Lấy UserId từ session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login"); // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
            }

            // Gọi UserService để lấy thông tin user từ API
            var user = await userService.GetUserProfileAsync(userId.Value);
            if (user == null)
            {
                return RedirectToAction("Index", "Login"); // Chuyển hướng nếu không tìm thấy user
            }

            return View(user); // Trả dữ liệu user về view
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login"); // Redirect to login if not logged in
            }

            var response = await userService.ChangePassword(userId.Value, currentPassword, newPassword, confirmNewPassword);
                
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Đổi mật khẩu thành công.";
                return RedirectToAction("Profile"); // Redirect back to profile page
            }
            else
            {
                ViewBag.Message = "Đổi mật khẩu thất bại. Vui lòng thử lại.";
                return View("Profile"); // Show the profile view with an error message
            }
        }


        public IActionResult Index()
        {



            return View();
        }
    }
}
