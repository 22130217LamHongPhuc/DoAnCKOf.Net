using API.Net.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Web.net.Service;

namespace Web.net.Controllers
{
    public class InforOrder : Controller
    {
        private readonly OrderService _orderService;

        public InforOrder(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> History(int? status)
        {
            var userId = HttpContext.Session.GetInt32("UserId"); // Lấy UserId từ session
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var orders = await _orderService.GetOrdersByUserIdAsync((int)userId);
            if (status.HasValue)
            {
                orders = orders.Where(o => o.OrderStatusId == status.Value).ToList();
            }
            ViewBag.Orders = orders;
            ViewBag.Fullname = orders.FirstOrDefault()?.Fullname ?? "Khách hàng";
            return View();
        }



        public IActionResult Index(string userEmail)
        {

            Order order = HttpContext.Session.GetObject<Order>("order");

            if (order != null)
            {
                ViewBag.Order = order;

            }

            return View();
        }

        public IActionResult BackToHome()
        {
           
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }



    }
}
