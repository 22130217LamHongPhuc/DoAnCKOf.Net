using API.Net.Models;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Web.net.Service
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<bool> CreateUserAsync(object user)
        {
            var jsonUser = JsonConvert.SerializeObject(user);
            var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5009/users/register", content);

            Console.WriteLine($"Status Code: {response.StatusCode}");
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");

            return response.IsSuccessStatusCode;
        }


        public async Task<User> GetLogin(string email, string password)
        {

            string url = "http://localhost:5009/users/login?email="+email +"&password="+password;

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);


                var user = System.Text.Json.JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // khong phan biet hoa thuong
                });

                if (user != null)
                {
                    return user;
                
                    
            }


        }
            return null;

        }

        public async Task<User> GetUserProfileAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5009/users/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(json);
            }
            return null;
        }

        public async Task<HttpResponseMessage> ChangePassword(int userId, string currentPassword, string newPassword, string confirmNewPassword)
        {
            var url = $"http://localhost:5009/users/change-password?userId={userId}&currentPassword={currentPassword}&newPassword={newPassword}&confirmNewPassword={confirmNewPassword}";
            var response = await _httpClient.PostAsync(url, null); // Send the request to the API
            return response;
        }

    }
}
