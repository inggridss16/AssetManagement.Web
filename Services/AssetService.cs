using AssetManagement.Web.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AssetManagement.Web.Services
{
    public class AssetService : IAssetService
    {
        private readonly HttpClient _httpClient;

        public AssetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AssetViewModel>> GetAssetsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/Assets/myAssets");
            response.EnsureSuccessStatusCode();

            var assets = await response.Content.ReadFromJsonAsync<IEnumerable<AssetViewModel>>();
            return assets ?? new List<AssetViewModel>();
        }
    }
}