// AssetManagement.Web/Services/IMaintenanceService.cs
using AssetManagement.Web.Models;
using System.Threading.Tasks;

namespace AssetManagement.Web.Services
{
    public interface IMaintenanceService
    {
        Task AddMaintenanceRecordAsync(string assetId, MaintenanceRecordViewModel maintenanceRecord, string token);
        Task<MaintenanceRecordViewModel> GetMaintenanceRecordByIdAsync(string assetId, long id, string token);
        Task UpdateMaintenanceRecordAsync(string assetId, long id, MaintenanceRecordViewModel maintenanceRecord, string token);
    }
}