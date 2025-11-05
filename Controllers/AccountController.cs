using AssetManagement.Web.Models;
using AssetManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AssetManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;

        // Inject both the authentication service and a logger.
        public AccountController(IAuthService authService, ILogger<AccountController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call the authentication service to validate credentials against the API.
                    var token = await _authService.LoginAsync(model.Username, model.Password);

                    if (!string.IsNullOrEmpty(token))
                    {
                        // Upon successful login, store the token in the session.
                        HttpContext.Session.SetString("JWToken", token);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // This handles the case where the API returns an unauthorized status.
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                        return View(model);
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Log the exception for debugging purposes.
                    _logger.LogError(ex, "An error occurred while communicating with the authentication API.");

                    // Add a user-friendly error message to the model state.
                    ModelState.AddModelError(string.Empty, "Unable to connect to the authentication service. Please try again later.");
                    return View(model);
                }
                catch (Exception ex)
                {
                    // Catch any other unexpected exceptions.
                    _logger.LogError(ex, "An unexpected error occurred during login.");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Clear the session to remove the authentication token.
            HttpContext.Session.Clear();
            // Redirect the user to the login page.
            return RedirectToAction("Login", "Account");
        }
    }
}