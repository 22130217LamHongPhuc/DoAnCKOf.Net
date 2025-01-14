using API.Net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Net.Controllers
{
    [Route("products")]  // Route 
    public class ProductController : Controller
    {
        private readonly DbtestContext _context;

        public ProductController(DbtestContext context)
        {
            _context = context;
        }


        

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _context.Products.Take(10).ToList();
            return Ok(products);

        }
       

        [HttpGet("new")]
        public IActionResult GetProductNew()
        {
            var products = _context.Products.ToList()
                .OrderByDescending(p => p.CreateAt).Take(10);
            return Ok(products);
        }
       


        [HttpGet("best_seller")]
        public IActionResult GetProductBestSeller()
        {
            var products = _context.Products.
                Include(p => p.OrderDetails)
                .ToList()
                .OrderByDescending(p => p.OrderDetails.Count())
                .Take(10);
            return Ok(products);
        }

        [HttpGet("highest_price")]
        public IActionResult GetProductHighestPrice()
        {
            var products = _context.Products
                .ToList()

                .OrderByDescending(p => p.Price).Take(10);
            return Ok(products);
        }
        [HttpGet("detail/{idProduct}")]
        public IActionResult GetProductById(string idProduct)
        {

            //var spec = _context.Specifications.Where(s=>s.SpecificationId == idProduct).FirstOrDefault();
            var product = _context.Products.Where(p=>p.ProductId == idProduct).FirstOrDefault();
            if(product == null)
            {
                return NotFound();
            }

            return Ok(product);


        }
        [HttpGet("image/{idProduct}")]
        public IActionResult GetImageById(string idProduct)
        {
            var subImage = _context.Subimages
                          .Where(i => i.ProductId == idProduct)
                          // Trả về danh sách các chuỗi (URLs)
                          .ToList();
            return Ok(subImage);
        }
       




        //    [HttpGet("{id}")]
        //public IActionResult GetById(string id)
        //{
        //    // Lấy một sản phẩm theo ID
        //    var product = _context.Products
        //     .Include(p => p.Subimages)     
        //     .Include(p => p.Subcategory)
        //      .Include(p => p.Specification)     
        //     .FirstOrDefault(p => p.ProductId == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(product);

        //}




    }
}
