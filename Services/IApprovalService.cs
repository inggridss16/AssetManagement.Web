using AssetManagement.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetManagement.Web.Services
{
    public interface IApprovalService
    {
        Task<IEnumerable<AssetViewModel>> GetPendingApprovalsAsync(string token);
    }
}