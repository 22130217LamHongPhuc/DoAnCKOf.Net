using System.Text.Json;
using API.Net.Models;

namespace Web.net.Service
{
    public class HomeService
    {

        private readonly HttpClient _httpClient;

        public HomeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<List<Subcategory>> GetSubCategory()
        {

            string url = "http://localhost:5009/home/subCategory";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
               var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var category = JsonSerializer.Deserialize<List<Subcategory>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });
                if (category != null)
                {
                    return category;
                }

            }
            return new List<Subcategory>();
        }
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            HomeService homeService = new HomeService(httpClient);

            List<Subcategory> list = await homeService.GetSubCategory();
            Console.WriteLine(list);
        }


    }
}
