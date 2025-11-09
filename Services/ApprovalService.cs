using AssetManagement.Web.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AssetManagement.Web.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly HttpClient _httpClient;

        public ApprovalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AssetViewModel>> GetPendingApprovalsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/workflow/pending");
            response.EnsureSuccessStatusCode();

            var assets = await response.Content.ReadFromJsonAsync<IEnumerable<AssetViewModel>>();
            return assets ?? new List<AssetViewModel>();
        }

        public async Task SubmitApprovalAsync(ApprovalDto approval, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync("/api/workflow/approve", approval);
            response.EnsureSuccessStatusCode();
        }
    }
}