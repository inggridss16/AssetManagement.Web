using AssetManagement.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetManagement.Web.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetViewModel>> GetAssetsAsync(string token);
    }
}