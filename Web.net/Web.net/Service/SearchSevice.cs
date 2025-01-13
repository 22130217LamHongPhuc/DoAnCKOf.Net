using API.Net.Models;
using System.Text.Json;

namespace Web.net.Service
{
    public class SearchSevice
    {
        private readonly HttpClient _httpClient;

        public SearchSevice(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        

        public async Task<List<Product>> GetProductBySearch(string keySearch)
        {
           

            string url = "http://localhost:5009/search?keySearch=" + keySearch;

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine("da in ra "+json);


                var product = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });
                
                    return product;
                

            }
            return new List<Product>();
        }
    }
}
