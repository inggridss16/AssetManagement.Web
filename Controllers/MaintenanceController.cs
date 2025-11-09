using AssetManagement.Web.Models;
using AssetManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AssetManagement.Web.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly ILogger<MaintenanceController> _logger;

        public MaintenanceController(IMaintenanceService maintenanceService, ILogger<MaintenanceController> logger)
        {
            _maintenanceService = maintenanceService;
            _logger = logger;
        }

        // GET: Maintenance/Create
        public IActionResult Create(string assetId)
        {
            var model = new MaintenanceRecordViewModel
            {
                LinkedAssetId = assetId,
                MaintenanceDate = DateTime.Now
            };
            return View(model);
        }

        // POST: Maintenance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceRecordViewModel model)
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
                    await _maintenanceService.AddMaintenanceRecordAsync(model.LinkedAssetId, model, token);
                    return RedirectToAction("Edit", "Asset", new { id = model.LinkedAssetId });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating the maintenance record.");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the maintenance record.");
                }
            }
            return View(model);
        }

        // GET: Maintenance/Edit/5
        public async Task<IActionResult> Edit(string assetId, long id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var model = await _maintenanceService.GetMaintenanceRecordByIdAsync(assetId, id, token);
                if (model == null)
                {
                    return NotFound();
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching maintenance record with id {id}.", id);
                return View("Error");
            }
        }

        // POST: Maintenance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MaintenanceRecordViewModel model)
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
                    await _maintenanceService.UpdateMaintenanceRecordAsync(model.LinkedAssetId, model.Id, model, token);
                    return RedirectToAction("Edit", "Asset", new { id = model.LinkedAssetId });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating the maintenance record.");
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the maintenance record.");
                }
            }
            return View(model);
        }

        // GET: Maintenance/Delete/5
        public async Task<IActionResult> Delete(string assetId, long id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var model = await _maintenanceService.GetMaintenanceRecordByIdAsync(assetId, id, token);

                // This check is crucial. If the record is not found, return a 404 page.
                if (model == null)
                {
                    return NotFound();
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching maintenance record for deletion.");
                return View("Error");
            }
        }

        // POST: Maintenance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string assetId, long id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                await _maintenanceService.DeleteMaintenanceRecordAsync(assetId, id, token);
                return RedirectToAction("Edit", "Asset", new { id = assetId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting maintenance record.");

                // Optionally, redirect to an error page or show an error message
                return View("Error");
            }
        }
    }
}