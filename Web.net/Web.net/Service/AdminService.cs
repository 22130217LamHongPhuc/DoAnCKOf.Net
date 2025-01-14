using API.net.ViewModel;
using API.Net.Models;
using API.Net.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace Web.net.Service
{
    public class AdminService
    {
        private readonly HttpClient _httpClient;

        public AdminService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Lấy danh sách sản phẩm
        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5009/AdminProducts");
            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });

                if (products != null)
                {
                    return products;
                }
            }

            return new List<Product>();
        }

        public async Task<Specification> GetProductSpecAsync(string id)
        {
            string url = $"http://localhost:5009/AdminSpecifications/{id}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var products = System.Text.Json.JsonSerializer.Deserialize<Specification>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });

                if (products != null)
                {
                    return products;
                }
            }

            return new Specification();
        }

        public async Task<List<Subimage>> GetImgAsync(string id)
        {
            string url = $"http://localhost:5009/AdminProducts/getImg/{id}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var products = System.Text.Json.JsonSerializer.Deserialize< List<Subimage>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });

                if (products != null)
                {
                    return products;
                }
            }

            return [];
        }

        public async Task<bool> UpdateProductSpec(UpdateSpecModel specification)
        {
            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5009/AdminSpecifications", specification);
            return response.IsSuccessStatusCode;
        }


        public async Task<Product> GetProductByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5009/AdminProducts/{id}");
            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var products = JsonSerializer.Deserialize<Product>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });

                if (products != null)
                {
                    return products;
                }
            }

            return new Product();
        }

        // Thêm mới một sản phẩm
        public async Task<bool> AddProductAsync(AddProductViewModel productModel)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5009/AdminProducts", productModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddSubImgAsync(AddSubImageViiewModel img)
        {
            var response = await _httpClient.PostAsJsonAsync($"http://localhost:5009/AdminProducts/getImg", img);
            return response.IsSuccessStatusCode;
        }

        // Cập nhật sản phẩm
        public async Task<bool> UpdateProductAsync(UpdateProductModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5009/AdminProducts", model);
            return response.IsSuccessStatusCode;
        }

        // Xóa sản phẩm
        public async Task<bool> DeleteProductAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5009/AdminProducts/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSubImgAsync(int[] arr)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5009/AdminProducts/delImg", arr);
            return response.IsSuccessStatusCode;
        }

        [HttpGet]
        public async Task<List<Subcategory>> GetSubcategoryAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5009/AdminSubcategories");
            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var subcategories = System.Text.Json.JsonSerializer.Deserialize<List<Subcategory>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (subcategories != null)
                {
                    return subcategories;
                }
            }

            return new List<Subcategory>();
        }

        internal async Task<List<Order>> GetOrderAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5009/Orders");
            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var products = JsonSerializer.Deserialize < List<Order>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });

                if (products != null)
                {
                    return products;
                }
            }

            return new List<Order>();
        }
    }
}
