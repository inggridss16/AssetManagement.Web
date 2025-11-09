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

        public async Task<IEnumerable<MaintenanceRecordViewModel>> GetMaintenanceRecordsByAssetIdAsync(string assetId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/assets/{assetId}/maintenance");
            response.EnsureSuccessStatusCode();

            var records = await response.Content.ReadFromJsonAsync<IEnumerable<MaintenanceRecordViewModel>>();
            return records ?? new List<MaintenanceRecordViewModel>();
        }

        public async Task<IEnumerable<AssetViewModel>> GetAssetsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/Assets/myAssets");
            response.EnsureSuccessStatusCode();

            var assets = await response.Content.ReadFromJsonAsync<IEnumerable<AssetViewModel>>();
            return assets ?? new List<AssetViewModel>();
        }

        public async Task<IEnumerable<UserViewModel>> GetUsersAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/Users");
            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserViewModel>>();
            return users ?? new List<UserViewModel>();
        }

        public async Task CreateAssetAsync(AssetViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // The API expects an object matching AssetCreateDto, so we create one from our view model.
            var assetToCreate = new
            {
                model.AssetName,
                model.Description,
                model.ResponsiblePersonId,
                model.Category,
                model.Subcategory
            };

            var response = await _httpClient.PostAsJsonAsync("/api/Assets", assetToCreate);
            response.EnsureSuccessStatusCode();
        }

        public async Task<AssetViewModel> GetAssetByIdAsync(string id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/Assets/{id}");
            response.EnsureSuccessStatusCode();

            var asset = await response.Content.ReadFromJsonAsync<AssetViewModel>();
            return asset;
        }

        public async Task UpdateAssetAsync(AssetViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsJsonAsync($"/api/Assets/{model.Id}", model);
            response.EnsureSuccessStatusCode();
        }

        public async Task AskForReviewAsync(string id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync($"/api/Assets/AskForReview/{id}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAssetAsync(string id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"/api/Assets/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task<IEnumerable<TrxAssetApprovalViewModel>> GetApprovalLogsByAssetIdAsync(string assetId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/Assets/{assetId}/approval-logs");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<TrxAssetApprovalViewModel>>() ?? new List<TrxAssetApprovalViewModel>();
        }
    }
}