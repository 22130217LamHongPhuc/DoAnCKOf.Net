using API.Net.EF.Models;
using API.Net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Net.Controllers
{


    [Route("product_review")]
    public class ProductReviewController : Controller
    {

        private readonly DbtestContext _context;

        public ProductReviewController(DbtestContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public IActionResult Index(string id)
        {
            var reviews = _context.ProductReviews
           .Include(p => p.User)
           .Include(p => p.Product)
           .Where(p => p.ProductId == id)
           .Select(p => new
           {
               p.Rating,
               FullName = p.User.FullName,
               p.CreatedAt,
               p.Comment,
               ProductName = p.Product.Name,
               QuantitySell = p.Product.QuanlitySell
           })
           .ToList(); 

            return Ok(reviews); 

        }


        
            [HttpPost]
            public IActionResult InsertCmt([FromBody] CommentDto dto)
            {
              

                try
                {
                ProductReview p = new ProductReview
                {
                    UserId = dto.UserId,
                    ProductId = dto.IdProduct,
                    Rating = dto.Rating,
                    Comment = dto.Comment,
                    PhoneNumber = 0,
                    CreatedAt = DateTime.Now,
                    Fullname = ""
                };
                   _context.ProductReviews.Add(p);
                _context.SaveChanges();
                    return Ok(new { success = true, message = "Thêm bình luận thành công." });
                }
                catch (Exception ex)
                {
                
                    Console.WriteLine($"Error: {ex.Message}");
                    return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi thêm bình luận." });
                }
            

        }
    }
}
