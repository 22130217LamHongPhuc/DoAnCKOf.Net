using API.Net.Models;
using System.Text.Json;
using Web.net.Models;

namespace Web.net.Service
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }




        public async Task<List<Product>> GelAllProduct()
        {
           
            string url = "http://localhost:5009/products";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
              
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);

             
               var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });

                if(products !=null)
                {
                    return products;
                }
            }

            return new List<Product>();
           
        }
       
        public async Task<List<Product>> GetProductTopSell()
        {
            string url = "http://localhost:5009/home/topSell";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });
                if (products != null) {

                    return products;
                    
                }
              


            }
            return new List<Product>();
        }
        public async Task<List<Product>> LoadMoreProduct(string keySearch, int f, int pi)
        {
            string url = "http://localhost:5009/search/"+ keySearch + "/"+f+"/"+pi;
            var response = await _httpClient.GetAsync(url);

            Console.WriteLine($"Status Code: {response.StatusCode}");
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });
                if (products != null)
                {

                    return products;

                }



            }
            Console.WriteLine("truong hop ko tra ve gi");
            return new List<Product>();

        }
        public async Task<List<Product>> FilterProduct(string keySearch, int f, int pr)
        {
            string url = "http://localhost:5009/search/filter/" + keySearch + "/" + f + "/" + pr;
            var response = await _httpClient.GetAsync(url);

            
            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });
                if (products != null)
                {

                    return products;

                }



            }
            Console.WriteLine("truong hop ko tra ve gi");
            return new List<Product>();

        }
        public async Task<List<Product>> GetProductFlashSales()
        {
            string url = "http://localhost:5009/home/flashSales";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
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


        public async Task<List<Product>> GetProductHighestDate()
        {

            string url = "http://localhost:5009/products/new";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
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
        public async Task<Product> GetProdByID(string id)
        {
            string url = "http://localhost:5009/products/detail/" + id;

            var response = await _httpClient.GetAsync(url);
           
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var productResponse = JsonSerializer.Deserialize<Product>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return productResponse;

            
        }
        public async Task<List<Subimage>> GetImageByID(string id)
        {
            string url = "http://localhost:5009/products/image/" + id;

            var response = await _httpClient.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);


            var productResponse = JsonSerializer.Deserialize<List<Subimage>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return productResponse;


        }
       


        public async Task<List<Product>> GetProductHighestPrice()
        {

            string url = "http://localhost:5009/products/highest_price";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
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


        //public async Task<List<Product>> GetProductBestSeller()
        //{

        //    string url = "http://localhost:5009/products/best_seller";

        //    var response = await _httpClient.GetAsync(url);

        //    if (response.IsSuccessStatusCode)
        //    {

        //        var json = await response.Content.ReadAsStringAsync();

        //        Console.WriteLine(json);

        //        //convert ve product
        //        var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
        //        });

        //        return new List<Product>();
        //    }
          

        //}


    }
}
