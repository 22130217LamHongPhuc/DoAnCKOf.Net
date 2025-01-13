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

            var query = _context.Products
                                .Where(p => p.Name.Contains(keySearch))
                                .Take(pi*20);  // Lấy đúng số lượng sản phẩm yêu cầu

            // Bước 2: Áp dụng filter/sort tùy theo giá trị của f
            query = SortProducts(query, f);

            // Bước 3: Trả về kết quả
            var result = query.ToList();

            return Ok(result);


        }

        //[HttpGet("filter/{keySearch}/{f}/{pr}")]
        //public IActionResult GetFilter(string keySearch, int f, int pr)
        //{
        //    List<Product> result = new List<Product>();
        //    switch (f)
        //    {
        //        case 0:
        //            result = _context.Products.Where(p => p.Name.Contains(keySearch)).Take(pr).ToList();
        //            break;
        //        case 1:

        //            result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderByDescending(p => p.QuanlitySell).Take(pr).ToList();
        //            break;
        //        case 2:
        //            result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderByDescending(p => p.DiscountDefault).Take(pr).ToList();
        //            break;
        //        case 3:
        //            result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderByDescending(p => (p.Price - (p.DiscountDefault/100.0)*p.Price)).Take(pr).ToList();
        //            break;
        //        case 4:
        //            result = _context.Products.Where(p => p.Name.Contains(keySearch)).OrderBy(p => (p.Price - (p.DiscountDefault/100.0) * p.Price)).Take(pr).ToList();
        //            break;
        //        default:
        //            return BadRequest("Khong hop le");

        //    }

        //    return Ok(result);

        //}
        [HttpGet("filter/{keySearch}/{f}/{pr}")]
        public IActionResult GetFilter(string keySearch, int f, int pr)
        {
            // Bước 1: Lấy sản phẩm chứa keySearch và giới hạn số lượng theo pr
            var query = _context.Products
                                .Where(p => p.Name.Contains(keySearch))
                                .Take(pr);  // Lấy đúng số lượng sản phẩm yêu cầu

            // Bước 2: Áp dụng filter/sort tùy theo giá trị của f
            query = SortProducts(query, f);

            // Bước 3: Trả về kết quả
            var result = query.ToList();

            return Ok(result);
        }
        private IQueryable<Product> SortProducts(IQueryable<Product> query, int f)
        {
            switch (f)
            {
                case 0:
                    // Không sắp xếp, giữ nguyên
                    return query;
                case 1:
                    // Sắp xếp theo QuanlitySell (bán chạy) giảm dần
                    return query.OrderByDescending(p => p.QuanlitySell);
                case 2:
                    // Sắp xếp theo DiscountDefault giảm dần
                    return query.OrderByDescending(p => p.DiscountDefault);
                case 3:
                    // Sắp xếp theo giá sau giảm giá (giá từ cao xuống thấp)
                    return query.OrderByDescending(p => (p.Price - (p.DiscountDefault / 100.0) * p.Price));
                case 4:
                    // Sắp xếp theo giá sau giảm giá (giá từ thấp đến cao)
                    return query.OrderBy(p => (p.Price - (p.DiscountDefault / 100.0) * p.Price));
                default:
                    throw new ArgumentException("Không hợp lệ");
            }
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
