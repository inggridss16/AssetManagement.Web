// AssetManagement.Web/Services/MaintenanceService.cs
using AssetManagement.Web.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
// Tambahkan using statement untuk logging jika diperlukan, contohnya:
// using Microsoft.Extensions.Logging;

namespace AssetManagement.Web.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly HttpClient _httpClient;
        // Jika Anda ingin melakukan logging, Anda bisa inject ILogger
        // private readonly ILogger<MaintenanceService> _logger;

        public MaintenanceService(HttpClient httpClient /*, ILogger<MaintenanceService> logger */)
        {
            _httpClient = httpClient;
            // _logger = logger;
        }

        public async Task AddMaintenanceRecordAsync(string assetId, MaintenanceRecordViewModel model, string token)
        {
            try
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
                response.EnsureSuccessStatusCode(); // Ini akan melempar HttpRequestException jika status code bukan 2xx
            }
            catch (HttpRequestException ex)
            {
                // Di sini Anda bisa melakukan logging terhadap error
                // _logger.LogError(ex, $"Gagal menambahkan catatan pemeliharaan untuk aset {assetId}.");

                // Anda bisa melempar kembali pengecualian ini atau menangani secara spesifik
                // tergantung pada bagaimana Anda ingin UI merespons
                throw new ApplicationException($"Terjadi kesalahan saat berkomunikasi dengan server: {ex.Message}", ex);
            }
        }

        public async Task<MaintenanceRecordViewModel> GetMaintenanceRecordByIdAsync(string assetId, long id, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"/api/assets/{assetId}/maintenance/{id}");
                response.EnsureSuccessStatusCode();

                var record = await response.Content.ReadFromJsonAsync<MaintenanceRecordViewModel>();
                return record;
            }
            catch (HttpRequestException ex)
            {
                // _logger.LogError(ex, $"Gagal mendapatkan catatan pemeliharaan dengan ID {id} untuk aset {assetId}.");
                throw new ApplicationException($"Gagal mengambil data dari server: {ex.Message}", ex);
            }
        }

        public async Task UpdateMaintenanceRecordAsync(string assetId, long id, MaintenanceRecordViewModel model, string token)
        {
            try
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
            catch (HttpRequestException ex)
            {
                // _logger.LogError(ex, $"Gagal memperbarui catatan pemeliharaan dengan ID {id} untuk aset {assetId}.");
                throw new ApplicationException($"Terjadi kesalahan saat memperbarui data: {ex.Message}", ex);
            }
        }

        public async Task DeleteMaintenanceRecordAsync(string assetId, long id, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.DeleteAsync($"/api/assets/{assetId}/maintenance/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                // _logger.LogError(ex, $"Gagal menghapus catatan pemeliharaan dengan ID {id} untuk aset {assetId}.");
                throw new ApplicationException($"Terjadi kesalahan saat menghapus data: {ex.Message}", ex);
            }
        }
    }
}