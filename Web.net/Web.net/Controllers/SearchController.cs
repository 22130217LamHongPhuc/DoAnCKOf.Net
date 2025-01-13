using Microsoft.AspNetCore.Mvc;
using Web.net.Service;

namespace Web.net.Controllers
{
    public class SearchController : Controller
    {

        private readonly ProductService _productService;
        private readonly HomeService _homeService;
        private readonly SearchSevice _searchSevice;


        public SearchController(ProductService productService, HomeService homeService, SearchSevice searchSevice)
        {
            _homeService = homeService;
            _productService = productService;
            _searchSevice = searchSevice;
        }
        public async Task<IActionResult> Index(string keySearch)
        {
            if (string.IsNullOrEmpty(keySearch))
            {
                ViewBag.Message = "Vui lòng nhập từ khóa ";
                return View("ProductSearch");
            }
            var product = await _searchSevice.GetProductBySearch(keySearch);
            if (product.Count == 0)
            {
                ViewBag.Message = "Không tìm thấy sản phẩm nào";
                return View("ProductSearch");
            }


            ViewBag.key = keySearch;
            ViewBag.Product = product;

            return View("ProductSearch");
        }

        public async Task<IActionResult> LoadProd(string keySearch, int f, int pi)
        {
            var listProduct = await _productService.LoadMoreProduct(keySearch, f, pi);

            return Json(new { products = listProduct });
        }
        public async Task<IActionResult> Filter(string keySearch, int f, int pr)
        {
            var listProduct = await _productService.FilterProduct(keySearch, f, pr);
            return Json(new { products = listProduct });


        } 

        }
}
