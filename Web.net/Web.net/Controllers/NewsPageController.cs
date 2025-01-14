using Microsoft.AspNetCore.Mvc;
using Web.net.Service;

namespace Web.net.Controllers
{
    public class NewsPageController : Controller
    {
        private readonly ProductService _productService;
        private readonly HomeService _homeService;
        public NewsPageController(ProductService productService, HomeService homeService)
        {
            _homeService = homeService;
            _productService = productService;

        }
        public async Task<IActionResult> Index()
        {
            return View("NewsPage");
        }
        public async Task<IActionResult> AddNews()
        {
            return View("AddNews");
        }

    }
}
