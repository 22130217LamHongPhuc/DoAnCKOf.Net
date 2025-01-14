using API.Net.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Web.net.Service
{
    public class ProductReviewService
    {

        private readonly HttpClient _httpClient;

        public ProductReviewService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductReview>> GetReviewByProductId(string id)
        {

            string url = "http://localhost:5009/product_review/" + id;

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var review = System.Text.Json.JsonSerializer.Deserialize<List<ProductReview>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });

                if (review != null)
                {
                    return review;
                }
            }

            return new List<ProductReview>();

        }

        public async Task<bool> CreateCmtAsync(int userid,string idProduct,int rating,string cmt)
        {

            var data = new
            {
                UserId = userid,
                IdProduct = idProduct,
                Rating = rating,
                Comment = cmt
            };

            var jsonOrder = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5009/product_review", content); // URL của Web API

            Console.WriteLine($"Status Code: {response.StatusCode}");
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");

            return response.IsSuccessStatusCode;
        }

    }
}
