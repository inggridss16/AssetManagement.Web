// AssetManagement.Web/Services/MaintenanceService.cs
using AssetManagement.Web.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AssetManagement.Web.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly HttpClient _httpClient;

        public MaintenanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddMaintenanceRecordAsync(string assetId, MaintenanceRecordViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var maintenanceRecordToCreate = new
            {
                model.MaintenanceCost,
                model.MaintenanceType,
                model.Comments,
                model.Vendor,
                model.MaintenanceDate
            };

            var response = await _httpClient.PostAsJsonAsync($"/api/assets/{assetId}/maintenance", maintenanceRecordToCreate);
            response.EnsureSuccessStatusCode();
        }
        public async Task<MaintenanceRecordViewModel> GetMaintenanceRecordByIdAsync(string assetId, long id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/assets/{assetId}/maintenance/{id}");
            response.EnsureSuccessStatusCode();

            var record = await response.Content.ReadFromJsonAsync<MaintenanceRecordViewModel>();
            return record;
        }
        public async Task UpdateMaintenanceRecordAsync(string assetId, long id, MaintenanceRecordViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var maintenanceRecordToUpdate = new
            {
                model.MaintenanceCost,
                model.MaintenanceType,
                model.Comments,
                model.Vendor,
                model.MaintenanceDate
            };

            var response = await _httpClient.PutAsJsonAsync($"/api/assets/{assetId}/maintenance/{id}", maintenanceRecordToUpdate);
            response.EnsureSuccessStatusCode();
        }
    }
}