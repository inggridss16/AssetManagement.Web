using System.Threading.Tasks;

namespace AssetManagement.Web.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string username, string password);
    }
}