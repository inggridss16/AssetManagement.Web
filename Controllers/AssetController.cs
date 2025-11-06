using AssetManagement.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AssetManagement.Web.Controllers
{
    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly ILogger<AssetController> _logger;

        public AssetController(IAssetService assetService, ILogger<AssetController> logger)
        {
            _assetService = assetService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var assets = await _assetService.GetAssetsAsync(token);
                return View("/Views/Asset/Index.cshtml",assets);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching assets from API.");
                return View("Error");
            }
        }
    }
}