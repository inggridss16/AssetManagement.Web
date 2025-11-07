using AssetManagement.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetManagement.Web.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetViewModel>> GetAssetsAsync(string token);
        Task<IEnumerable<UserViewModel>> GetUsersAsync(string token);
        Task CreateAssetAsync(AssetViewModel asset, string token);
        Task<AssetViewModel> GetAssetByIdAsync(string id, string token);
        Task UpdateAssetAsync(AssetViewModel asset, string token);
    }
}