using API.Net.Models;
using ECommerceMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using Web.net.Models;
using Web.net.Service;
using static Web.net.Models.PaymentResponseModel;
using System.Text;

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
                UserId = 10,
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



                await SendMailGoogleSmtp("tailam164@gmail.com", Convert.ToString(userInfo.Email), "Thông Tin Đơn Hàng", CreateOrderEmailBody(order),
                                         "tailam164@gmail.com", "bdnpbtgnmmelluak");





                HttpContext.Session.SetObject("order", order);

                Order order2 = HttpContext.Session.GetObject<Order>("order");


                ViewBag.Cart = cart;

                return RedirectToAction("Index", "InforOrder", new { userEmail = userInfo.Email });
            }

            return Ok();


        }


        public static async Task<bool> SendMailGoogleSmtp(string _from, string _to, string _subject,
                                                          string _body, string _gmailsend, string _gmailpassword)
        {

            MailMessage message = new MailMessage(
                from: _from,
                to: _to,
                subject: _subject,
                body: _body
            );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);

            // Tạo SmtpClient kết nối đến smtp.gmail.com
            using var client = new SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.Credentials = new NetworkCredential(_gmailsend, _gmailpassword);
            client.EnableSsl = true;

            try
            {
                await client.SendMailAsync(message);
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);


                return false;
            }

        }

        public static string CreateOrderEmailBody(Order order)
        {
            StringBuilder emailBody = new StringBuilder();

            emailBody.Append(@"
            <style>
                body {
                    font-family: Arial, sans-serif;
                    color: #333333;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;

                }
                .container {
                    background-color: #fffef6;
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    font-size: 16px;
                    border-radius: 10px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }
                h1 {
                    color: #4CAF50;
                    text-align: center;
                }
                p {
                    font-size: 16px;
                    line-height: 1.5;
                }
                table {
                    width: 100%;
                    border-collapse: collapse;
                    margin: 20px 0;
                }
                th, td {
                    padding: 12px;
                    text-align: left;
                    border-bottom: 1px solid #ddd;
                }
                th {
                    background-color: #f2f2f2;
                    color: #333;
                }
                td {
                    background-color: #fafafa;
                }
                .footer {
                    text-align: center;
                    font-size: 14px;
                    color: #777;
                    margin-top: 30px;
                }
                .total-price {
                    font-weight: bold;
                    color: #4CAF50;
                }
                .order-status {
                    padding: 8px 12px;
                    background-color: #ff9800;
                    color: white;
                    border-radius: 4px;
                    font-weight: bold;
                }
            </style>
            ");

            emailBody.Append("<div class=\"container\" style=\" background-color: #fffef6;font-size: 16px;border-radius: 10px; \" > ");
            emailBody.Append("<h1>Thông tin đơn hàng</h1>");

            emailBody.Append("<p><strong>Họ tên: </strong>" + order.Fullname + "</p>");
            emailBody.Append("<p><strong>Email: </strong>" + order.Email + "</p>");
            emailBody.Append("<p><strong>Số điện thoại: </strong>" + order.PhoneNumber + "</p>");
            emailBody.Append("<p><strong>Địa chỉ giao hàng: </strong>" + order.Address + "</p>");
            emailBody.Append("<p><strong>Ghi chú: </strong>" + order.Note + "</p>");

            emailBody.Append("<p><strong>Ngày tạo đơn: </strong>" + order.CreateAt.ToString("dd/MM/yyyy HH:mm:ss") + "</p>");

            emailBody.Append("<h3>Chi tiết sản phẩm:</h3>");
            emailBody.Append("<table>");
            emailBody.Append("<tr><th>Sản phẩm</th><th>Số lượng</th><th>Tổng giá</th></tr>");

            foreach (var item in order.Items)
            {
                emailBody.Append("<tr>");
                emailBody.Append("<td>" + item.ProductId + "</td>");
                emailBody.Append("<td>" + item.Quantity + "</td>");
                emailBody.Append("<td>" + item.TotalPrice.ToString("C") + "</td>");
                emailBody.Append("</tr>");
            }




            emailBody.Append("<tr><td colspan='2'><strong>Tổng giá : </strong></td><td class='total-price'>" + order.TotalPrice.ToString("C") + "</td></tr>");
            emailBody.Append("<tr><td colspan='2'><strong>Phí vận chuyển :</strong></td><td class='total-price'>" + order.fee + "</td></tr>");
            emailBody.Append("<tr><td colspan='2'><strong>Giảm giá :</strong></td><td class='total-price'>" + order.discount + "</td></tr>");
            emailBody.Append("<tr><td colspan='2'><strong>Tổng giá trị đơn hàng:</strong></td><td class='total-price'>" + (order.TotalPrice - order.discount + order.fee) + "</td></tr>");




            emailBody.Append("<p><strong>Phương thức thanh toán: </strong><span class='order-status'>" + order.PaymentName + "</span></p>");

            emailBody.Append("<p><strong>Trạng thái đơn hàng: </strong><span class='order-status'>" + order.OrderStatusName + "</span></p>");


            if (!string.IsNullOrEmpty(order.DeliveredDate))
            {
                emailBody.Append("<p><strong>Ngày giao hàng: </strong>" + order.DeliveredDate + "</p>");
            }
            else
            {
                emailBody.Append("<p><strong>Đơn hàng chưa được giao.</strong></p>");
            }

            emailBody.Append("<p class='footer'>Cảm ơn bạn đã mua hàng tại cửa hàng chúng tôi!</p>");
            emailBody.Append("</div>");

            return emailBody.ToString();
        }








    }
}
