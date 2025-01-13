using API.Net.Models;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Web.net.Service;

namespace Web.net.Controllers
{
    public class ProductsController : Controller
    {

        private readonly ProductService _productService;
        private readonly HomeService _homeService;

        public ProductsController(ProductService productService)
        {

            _productService = productService;

        }

        public async Task<IActionResult> Detail(string id)
        {
            Product detail = await _productService.GetProdByID(id);


            var image = await _productService.GetImageByID(id);
            ViewBag.Image = image;
            ViewBag.Detail = detail;
            return View("ProductDetail");

        }

    }
}
