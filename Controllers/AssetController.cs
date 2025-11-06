using AssetManagement.Web.Models;
using AssetManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                return View("/Views/Asset/Index.cshtml", assets);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching assets from API.");
                return View("Error");
            }
        }

        // GET: Asset/Create
        // This method displays the form to create a new asset.
        public IActionResult Create()
        {
            // First, check if the user is logged in.
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If not, redirect them to the login page.
                return RedirectToAction("Login", "Account");
            }

            // Create a new view model to hold the data for the form.
            var viewModel = new AssetViewModel
            {
                // Initialize the list for category options.
                CategoryOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "IT Equipment", Text = "IT Equipment" },
                    new SelectListItem { Value = "Furniture", Text = "Furniture" }
                },
                // Initialize the subcategory options list as empty. It will be populated by JavaScript.
                SubcategoryOptions = new List<SelectListItem>()
            };

            // Pass the view model to the view.
            return View(viewModel);
        }

        // POST: Asset/Create
        // This method is called when the user submits the form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssetViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Call the service to create the new asset via the API.
                    // Note: You will need to create this 'CreateAssetAsync' method in your IAssetService and AssetService.
                    // await _assetService.CreateAssetAsync(model, token);

                    // After successfully creating, redirect back to the list of assets.
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating the asset.");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the asset.");
                }
            }

            // If the model is not valid, repopulate the dropdowns and return to the form
            model.CategoryOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "IT Equipment", Text = "IT Equipment" },
                new SelectListItem { Value = "Furniture", Text = "Furniture" }
            };
            model.SubcategoryOptions = new List<SelectListItem>(); // Repopulate if necessary

            return View(model);
        }
    }
}