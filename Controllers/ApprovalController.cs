using AssetManagement.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AssetManagement.Web.Controllers
{
    public class ApprovalController : Controller
    {
        private readonly IApprovalService _approvalService;
        private readonly ILogger<ApprovalController> _logger;

        public ApprovalController(IApprovalService approvalService, ILogger<ApprovalController> logger)
        {
            _approvalService = approvalService;
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

            if (userRole != "manager")
            {
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
    }
}