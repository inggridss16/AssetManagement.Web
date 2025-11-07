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
    }
}