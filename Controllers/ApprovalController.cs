using AssetManagement.Web.Models;
using AssetManagement.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Web.Controllers
{
    public class ApprovalController : Controller
    {
        private readonly IApprovalService _approvalService;
        private readonly IAssetService _assetService;
        private readonly ILogger<ApprovalController> _logger;

        public ApprovalController(IApprovalService approvalService, IAssetService assetService, ILogger<ApprovalController> logger)
        {
            _approvalService = approvalService;
            _assetService = assetService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            // Simple role check; in a real app, this might be more robust.
            if (userRole?.ToLower() != "manager")
            {
                // You might want to redirect to an "Access Denied" page
                return Forbid();
            }

            try
            {
                var pendingAssets = await _approvalService.GetPendingApprovalsAsync(token);
                return View(pendingAssets);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching pending approvals.");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Review(string id)
        {
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

                // Fetch maintenance records for the asset
                asset.MaintenanceRecords = await _assetService.GetMaintenanceRecordsByAssetIdAsync(id, token);

                var users = await _assetService.GetUsersAsync(token);
                asset.ResponsiblePersonOptions = users.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name,
                    Selected = u.Id == asset.ResponsiblePersonId
                }).ToList();

                return View(asset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching asset for review with ID {AssetId}", id);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(ApprovalSubmitViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid submission.";
                return RedirectToAction(nameof(Review), new { id = model.AssetId });
            }

            var approvalDto = new ApprovalDto
            {
                AssetId = model.AssetId,
                IsApproved = model.Action == "Approve",
                Comments = model.Comments ?? string.Empty
            };

            try
            {
                await _approvalService.SubmitApprovalAsync(approvalDto, token);
                TempData["SuccessMessage"] = $"Asset {model.AssetId} has been successfully {(approvalDto.IsApproved ? "approved" : "rejected")}.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting asset review for Asset ID {AssetId}", model.AssetId);
                TempData["ErrorMessage"] = "An error occurred while submitting the review.";
                return RedirectToAction(nameof(Review), new { id = model.AssetId });
            }
        }
    }
}