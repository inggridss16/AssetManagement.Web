// AssetManagement.Web/Controllers/MaintenanceController.cs
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
    }
}