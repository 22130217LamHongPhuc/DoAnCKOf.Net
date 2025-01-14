using API.Net.Models;
using API.Net.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Net.Controllers
{


    [Route("order")]
    public class OrderController : Controller
    {

        private readonly DbtestContext _context;

        public OrderController(DbtestContext context)
        {
            _context = context;
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetOrdersByUserId(int userId)
        {
            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.OrderStatus) // Include bảng OrderStatus
                .Select(o => new
                {
                    o.OrderId,
                    o.UserId,
                    o.TotalPrice,
                    o.OrderStatusId,
                    OrderStatusName = o.OrderStatus.NameStatus, // Thêm tên trạng thái
                    FullName = o.Fullname,
                    Items = o.OrderDetails.Select(od => new
                    {
                        od.ProductId,
                        od.Quantity,
                        od.TotalPrice
                    }).ToList()
                })
                .ToList();

            return Ok(orders);
        }

        [HttpPut("update-status")]
        public IActionResult UpdateOrderStatus([FromBody] UpdateOrderStatusModel model)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == model.OrderId);
            if (order == null)
            {
                return NotFound("Order not found");
            }

            order.OrderStatusId = model.OrderStatusId;
            _context.SaveChanges();
            return Ok("Order status updated successfully");
        }



        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderModel orderModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var order = new Order
                    {
                        UserId = orderModel.UserId,
                        Fullname = orderModel.Fullname,
                        Email = orderModel.Email,
                        PhoneNumber = orderModel.PhoneNumber,
                        Address = orderModel.Address,
                        PaymentId = orderModel.PaymentId,
                        TotalPrice = orderModel.TotalPrice,
                        OrderStatusId = orderModel.OrderStatusId,
                        Note = orderModel.Note,
                        fee = orderModel.fee,
                        discount = orderModel.discount
                    };
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    foreach (var item in orderModel.Items)
                    {
                        _context.OrderDetails.Add(new OrderDetail
                        {
                            OrderId = order.OrderId, // Đã có OrderId từ database sau khi lưu Order
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            TotalPrice = item.TotalPrice
                        });
                    }

                    _context.SaveChanges(); // Lưu OrderDetails

                    transaction.Commit();

                    return Ok("create success order ");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, "An error : " + ex.Message);
                }
            }
        }


    }

}
