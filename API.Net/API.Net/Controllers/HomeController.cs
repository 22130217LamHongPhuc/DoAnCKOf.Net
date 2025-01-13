using API.Net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Net.Controllers
{
    [Route("home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly DbtestContext _context;

        public HomeController(DbtestContext context)
        {
            _context = context;
        }
        // GET: api/<HomeController>
        [HttpGet("flashSales")]
        public IActionResult GetTop10ProdByFlashSales()
        {
            var products = _context.Products.OrderByDescending(p => p.DiscountDefault).Take(10).ToList();
            return Ok(products);

        }
        [HttpGet("subCategory")]
        public IActionResult GetAllSubCategory()
        {
            var subcategories = _context.Subcategories.ToList();

            return Ok(subcategories);

        }
        [HttpGet("topSell")]
        public IActionResult GetTop10ProdByTopSell()
        {
            var products = _context.Products.OrderByDescending(p => p.QuanlitySell).Take(10).ToList();

            return Ok(products);

        }
        [HttpGet("comboProduct")]
        public IActionResult GetTop10ProdByCombo()
        {
            var products = _context.Products.Where(p => p.Name.Contains("Combo")).Take(10).ToList();

            return Ok(products);

        }
       




    }
}
