using System.Linq;
using API.Net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Net.Controllers
{

    [Route("search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly DbtestContext _context;

        public SearchController(DbtestContext context)
        {
            _context = context;
        }
        [HttpGet("{keySearch}/{f}/{pi}")]
        public IActionResult GetProduct(string keySearch, int f, int pi)
        {

            List<Product> result =new List<Product>();

            switch (f)
            {
                case 0:
                    result = _context.Products.Where(p => p.Name.Contains(keySearch)).Skip(pi * 20).Take(20).ToList();
                    break;
                case 1:

                    result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderByDescending(p => p.QuanlitySell).Skip(pi * 20).Take(20).ToList();
                    break;
                case 2:
                    result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderByDescending(p => p.DiscountDefault).Skip(pi * 20).Take(20).ToList();
                    break;
                case 3:
                    result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderByDescending(p => p.Price).Skip(pi * 20).Take(20).ToList();
                    break;
                case 4:
                    result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderBy(p => p.Price).Skip(pi * 20).Take(20).ToList();
                    break;
                default:
                    return BadRequest("Khong hop le");

            }
            
            return Ok(result);


        }
        
        [HttpGet("filter/{keySearch}/{f}/{pr}")]
        public IActionResult GetFilter(string keySearch, int f, int pr)
        {
            List<Product> result = new List<Product>();
            switch (f)
            {
                case 0:
                    result = _context.Products.Where(p => p.Name.Contains(keySearch)).Take(pr).ToList();
                    break;
                case 1:

                    result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderByDescending(p => p.QuanlitySell).Take(pr).ToList();
                    break;
                case 2:
                    result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderByDescending(p => p.DiscountDefault).Take(pr).ToList();
                    break;
                case 3:
                    result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderByDescending(p => (p.Price - (p.DiscountDefault/100.0)*p.Price)).Take(pr).ToList();
                    break;
                case 4:
                    result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderBy(p => (p.Price - (p.DiscountDefault/100.0) * p.Price)).Take(pr).ToList();
                    break;
                default:
                    return BadRequest("Khong hop le");

            }

            return Ok(result);

        }

        // GET: api/<SearchController>
        [HttpGet]
        public IActionResult Get(string keySearch)
        {
            
            var products =  _context.Products
    .Where(p => p.Name.Contains(keySearch))
    .Select(p => new
    {
        p.ProductId,
        p.Name,
        p.Price,
        p.DiscountDefault,
        p.Description,
        p.SubcategoryId,
        p.QuanlityStock,
        p.QuanlitySell,
        p.Thumbnail,
        p.CreateAt
    })
    .Take(20)  // Giới hạn số lượng sản phẩm trả về là 20
    .ToList();

            if (products.Count == 0)
            {
                // Trả về mã 404 Not Found hoặc một thông báo tùy chỉnh
                return NotFound(new { message = "Không tìm sản phẩm nào." });
            }

            //Trả về danh sách sản phẩm nếu có dữ liệu
            return Ok(products);
        
        }

        [HttpGet("category/{subCategoryID}")]
        public IActionResult GetProdByCategory(string subCategoryID) { 
            var products = _context.Products.Where(p => p.SubcategoryId.Contains(subCategoryID)).Take(20).ToList();
            return Ok(products);


        }
        [HttpGet("category")]
        public IActionResult GetTop10ProdByCategory()
        { 
            var products = _context.Products.OrderByDescending(p => p.DiscountDefault).Take(10).ToList();
            return Ok(products);

        }



            // GET api/<SearchController>/5
            [HttpGet("{id}")]
       

        // POST api/<SearchController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SearchController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SearchController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
