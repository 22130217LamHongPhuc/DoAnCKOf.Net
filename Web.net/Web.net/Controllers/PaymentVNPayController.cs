using API.Net.Models;
using ECommerceMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using Web.net.Models;
using Web.net.Service;
using static Web.net.Models.PaymentResponseModel;

namespace Web.net.Controllers
{
    public class PaymentVNPayController : Controller
    {

        private readonly IVnPayService _vnPayService;
        private readonly OrderService _orderService;


        public PaymentVNPayController(IVnPayService vnPayService, OrderService orderService)
        {

            _vnPayService = vnPayService;
            _orderService = orderService;
        }

        public IActionResult Index(PaymentInformationModel model) {

            var cart = HttpContext.Session.GetObject<Cart>("cart");

            var vnPayModel = new VnPaymentRequestModel
            {
                Amount = (cart.TotalPriceCartAddFee() - cart.discount),
                CreatedDate = DateTime.Now,
                Description = "okla",
                FullName = "aaa",
                OrderId = new Random().Next(1000, 100000)
            };
        
            return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
        }


        public  async Task<IActionResult> PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }


            var userInfoJson = TempData["UserInfo"] as string;

            var userInfo = string.IsNullOrEmpty(userInfoJson) ? null :
                JsonConvert.DeserializeObject<dynamic>(userInfoJson);

            if (userInfo == null)
            {
                return RedirectToAction("Index", "InforPayment");
            }

            var cart = HttpContext.Session.GetObject<Cart>("cart");
            Order order;



            List<OrderDetail> orders = new List<OrderDetail>();


            foreach (var c in cart.getAllCartItem())
            {
                orders.Add(new OrderDetail(c.ProductID, c.Quatity, (decimal)c.TotalPriceCartItem()));
            }
            order = new Order
            {
                UserId = 1,
                Fullname = userInfo.Name,
                Email = userInfo.Email,
                PhoneNumber = userInfo.PhoneNumber,
                Address = userInfo.Address,
                PaymentName = "vnpay",
                PaymentId = 2,
                OrderStatusId = 1,
                OrderStatusName = "Chờ xác nhận",
                TotalPrice = (decimal)cart.TotalPriceCart(),
                CreateAt = DateTime.Now,
                Items = orders,
                fee = cart.fee,
                discount = cart.discount,
                Note = userInfo.Note
            };



            var isSuccess = await _orderService.CreateOrderAsync(order);



            if (isSuccess && order != null)
            {



                //await SendMailGoogleSmtp("tailam164@gmail.com", Convert.ToString(userInfo.Email), "Thông Tin Đơn Hàng", CreateOrderEmailBody(order),
                //                         "tailam164@gmail.com", "bdnpbtgnmmelluak");





                HttpContext.Session.SetObject("order", order);

                Order order2 = HttpContext.Session.GetObject<Order>("order");


                ViewBag.Cart = cart;

                return RedirectToAction("Index", "InforOrder", new { userEmail = userInfo.Email });
            }

            return Ok();


        }








    }
}
