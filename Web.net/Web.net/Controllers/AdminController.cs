using API.net.ViewModel;
using API.Net.Models;
using API.Net.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using Web.net.Service;

namespace Web.net.Controllers
{
 
    public class AdminController(AdminService adminService) : Controller
    {

        private AdminService _adminService = adminService;

        [HttpGet]
        public async Task<JsonResult> GetProduct()
        {
            List<Product> ls = await _adminService.GetProductsAsync();

            return Json(new {products = ls});

        }

        [HttpPost]
        public async Task<JsonResult> GetProductSpec()
        {

            string specId = Request.Form["specId"];
            Specification spec = await _adminService.GetProductSpecAsync(specId);

            return Json(new { success = true, spec }); 

        }

        [HttpGet]
        public async Task<JsonResult> GetImg()
        {

            string productId = Request.Query["pid"].ToString();
            List<Subimage> sub = await _adminService.GetImgAsync(productId);

            return Json(new { subImg = sub });

        }

        [HttpPost]
        public async Task<JsonResult> UpdateSpec()
        {

            String data = Request.Form["data"];
            var model = JsonConvert.DeserializeObject<UpdateSpecModel>(data);

            return Json(new { success = await _adminService.UpdateProductSpec(model)});

        }

        [HttpPost]
        public async Task<JsonResult> AddImg()
        {

            string data = Request.Form["data"];

            var model = JsonConvert.DeserializeObject<AddSubImageViiewModel>(data);

            return Json(new { success = await _adminService.AddSubImgAsync(model) });

        }

        [HttpPost]
        public async Task<JsonResult> DelImg()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                int[] numbers = JsonConvert.DeserializeObject<int[]>(body);

                return Json(new { success = await _adminService.DeleteSubImgAsync(numbers) });

            }

        }


        [HttpPost]
        public async Task<JsonResult> UpdateProduct()
        {
            string data = Request.Form["data"];
            var model = JsonConvert.DeserializeObject<UpdateProductModel>(data);
            return Json(new { success = await _adminService.UpdateProductAsync(model) });
            

        }

        public async Task<IActionResult> ManageProduct()
        {
            ViewBag.Subcategories = await _adminService.GetSubcategoryAsync();
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.Subcategories = await _adminService.GetSubcategoryAsync();
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            if(ModelState.IsValid)
            {
                bool success = await _adminService.AddProductAsync(model);
                return RedirectToAction(nameof(ManageProduct));
            }
            ViewBag.Subcategories = await _adminService.GetSubcategoryAsync();
            ViewBag.Model = model;
            return View();

        }

        [HttpGet]
        public async Task<JsonResult> GetOrder()
        {
            List<Order> ls = await _adminService.GetOrderAsync();
            return Json(new { orders = ls });

        }

        [HttpGet]
        public async Task<IActionResult> Order()
        {
            return View();

        }

        [HttpGet]
        public async  Task<JsonResult> GetOrderDetail() 
        {
            string productId = Request.Query["orderId"].ToString();
            List<Product> ls = await _adminService.GetOrderProduct(Int32.Parse(productId));
            return Json(new { success = true, orderdetail = ls });
        }


        [HttpGet]
        public async Task<JsonResult> UpdateOrder()
        {
            string id = Request.Query["id"].ToString();
            string active = Request.Query["active"].ToString();
            int realAcive = Int32.Parse(active);
            int[] numbers = JsonConvert.DeserializeObject<int[]>(id);
            //List<Product> ls = await _adminService.GetOrderProduct(Int32.Parse(productId));
            //return Json(new { success = true, orderdetail = ls });
            return Json(new { success = await _adminService.UpdateOrderStatus(numbers, realAcive)});
        }

    }
}
