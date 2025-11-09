using AssetManagement.Web.Models;
using AssetManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var users = await _assetService.GetUsersAsync(token);
            var responsiblePersonOptions = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Name
            }).ToList();

            var viewModel = new AssetViewModel
            {
                CategoryOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "IT Equipment", Text = "IT Equipment" },
                    new SelectListItem { Value = "Furniture", Text = "Furniture" }
                },
                SubcategoryOptions = new List<SelectListItem>(),
                ResponsiblePersonOptions = responsiblePersonOptions
            };

            return View(viewModel);
        }

        // POST: Asset/Create
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
                    await _assetService.CreateAssetAsync(model, token);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating the asset.");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the asset.");
                }
            }

            var users = await _assetService.GetUsersAsync(token);
            model.ResponsiblePersonOptions = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Name
            }).ToList();

            model.CategoryOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "IT Equipment", Text = "IT Equipment" },
                new SelectListItem { Value = "Furniture", Text = "Furniture" }
            };
            model.SubcategoryOptions = new List<SelectListItem>();

            return View(model);
        }

        // GET: Asset/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var asset = await _assetService.GetAssetByIdAsync(id, token);
                if (asset == null)
                {
                    return NotFound();
                }

                asset.MaintenanceRecords = await _assetService.GetMaintenanceRecordsByAssetIdAsync(id, token);

                var users = await _assetService.GetUsersAsync(token);
                asset.ResponsiblePersonOptions = users.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name,
                    Selected = u.Id == asset.ResponsiblePersonId
                }).ToList();

                asset.CategoryOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "IT Equipment", Text = "IT Equipment" },
                    new SelectListItem { Value = "Furniture", Text = "Furniture" }
                };

                return View(asset);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching asset with id {id} from API.", id);
                return View("Error");
            }
        }

        // POST: Asset/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AssetViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _assetService.UpdateAssetAsync(model, token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating the asset with id {id}.", id);
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the asset.");

                    var users = await _assetService.GetUsersAsync(token);
                    model.ResponsiblePersonOptions = users.Select(u => new SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = u.Name
                    }).ToList();
                    model.CategoryOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "IT Equipment", Text = "IT Equipment" },
                        new SelectListItem { Value = "Furniture", Text = "Furniture" }
                    };
                    model.SubcategoryOptions = new List<SelectListItem>();
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }

            var users2 = await _assetService.GetUsersAsync(token);
            model.ResponsiblePersonOptions = users2.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Name
            }).ToList();
            model.CategoryOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "IT Equipment", Text = "IT Equipment" },
                new SelectListItem { Value = "Furniture", Text = "Furniture" }
            };
            model.SubcategoryOptions = new List<SelectListItem>();
            return View(model);
        }

        // POST: Asset/AskForReview/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AskForReview(string id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Calls the service to update the asset's status
                await _assetService.AskForReviewAsync(id, token);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while asking for review for asset with id {id}.", id);
                TempData["ErrorMessage"] = "An error occurred while submitting the asset for review.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Asset/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var asset = await _assetService.GetAssetByIdAsync(id, token);
                if (asset == null)
                {
                    return NotFound();
                }

                // Fetch and add maintenance records to the view model
                asset.MaintenanceRecords = await _assetService.GetMaintenanceRecordsByAssetIdAsync(id, token);

                return View(asset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching asset for deletion with id {id}.", id);
                return View("Error");
            }
        }

        // POST: Asset/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                await _assetService.DeleteAssetAsync(id, token);
                TempData["SuccessMessage"] = "Asset deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting asset with id {id}.", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the asset.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}