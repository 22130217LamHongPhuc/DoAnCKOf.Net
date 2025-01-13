using API.Net.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Web.net.Models;
using Web.net.Service;

namespace Web.net.Controllers
{
    public class HomeController : Controller
    {

        private readonly ProductService _productService;
        private readonly HomeService _homeService;

        public HomeController(ProductService productService, HomeService homeService)
        {
            _homeService = homeService;
            _productService = productService;

        }

        public async Task<IActionResult> Index()
        {
            ViewBag.FlashSales = await _productService.GetProductFlashSales();
            

            ViewBag.ProductsDate = await _productService.GetProductHighestDate();
            ViewBag.ProductsPrice = await _productService.GetProductHighestPrice();
            ViewBag.topSell = await _productService.GetProductTopSell();
          
                 ViewBag.listCate = await _homeService.GetSubCategory();




            return View();

        }


       
   

}
}
