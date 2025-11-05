using AssetManagement.Web.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AssetManagement.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var loginViewModel = new LoginViewModel
            {
                Username = username,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginViewModel);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return loginResponse?.Token;
            }

            return null;
        }
    }

    // Helper class to deserialize the token from the API response
    internal class LoginResponse
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }
}