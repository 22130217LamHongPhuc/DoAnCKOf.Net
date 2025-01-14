using API.Net.Models;
using Microsoft.AspNetCore.Mvc;

using Web.net.Service;

namespace Web.net.Controllers
{
    public class ReviewProductController : Controller
    {

        private readonly ProductReviewService _Service;


        public ReviewProductController(ProductReviewService Service)
        {

            _Service = Service;
        }


        public async Task<IActionResult> Index(string id)
        {

            List<ProductReview> reviews = await _Service.GetReviewByProductId(id);

            ViewBag.reviews = reviews;
            ViewBag.ProductID = id;
            ViewBag.ProductName = reviews[0].ProductName;
            ViewBag.QuantitySold = reviews[0].QuantitySell;
            double averageRating = reviews.Average(r => r.Rating);
            ViewBag.AverageRating = Math.Round(averageRating, 1); 
            return View("ReviewProduct");


        }



        [HttpPost]

        public async Task<IActionResult> Comment(string id, int rating, string content)
        
        {
            //User user = (User) HttpContext.Session.GetObject<User>("user");
            // if (user == null)
            // {
            //     return Json(new
            //     {
            //         islogin =false
            //     });
            // }

            bool check=await _Service.CreateCmtAsync(1, id, rating, content);

            if(check) return Json(new { success = true, fullName = "Lam tai", createAt = DateTime.Now.ToString("dd / MM / yyyy") });


            return Json(new { success = false }) ;


        }
    }
}
