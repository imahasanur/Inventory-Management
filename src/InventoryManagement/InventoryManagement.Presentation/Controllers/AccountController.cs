using Autofac;
using InventoryManagement.Data.Membership;
using InventoryManagement.Presentation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AccountController(ILifetimeScope scope,
           ILogger<AccountController> logger,
           SignInManager<ApplicationUser> signInManager,
           UserManager<ApplicationUser> userManager,
           IConfiguration configuration,
           ITokenService tokenService)
        {
            _scope = scope;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }
        public IActionResult Register()
        {
            var model = _scope.Resolve<RegistrationModel>();
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            string captchaInput = model.CaptchaInput;
            string expectedCaptcha = HttpContext.Session.GetString("CaptchaCode");

            if (ModelState.IsValid && string.Equals(expectedCaptcha, captchaInput, StringComparison.OrdinalIgnoreCase))
            {

                model.Resolve(_scope);
                var response = await model.RegisterAsync(Url.Content("~/"));

                if (response.errors is not null)
                {
                    foreach (var error in response.errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        _logger.LogError(error.Description);
                    }
                }
                else
                    return Redirect(response.redirectLocation);
                //return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var model = _scope.Resolve<LoginModel>();

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            string captchaInput = model.CaptchaInput;
            string expectedCaptcha = HttpContext.Session.GetString("CaptchaCode");

            if (ModelState.IsValid && string.Equals(expectedCaptcha, captchaInput, StringComparison.OrdinalIgnoreCase))
            {

                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var claims = (await _userManager.GetClaimsAsync(user)).ToArray();
                    var token = await _tokenService.GetJwtToken(claims,
                            _configuration["Jwt:Key"],
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"]
                        );
                    HttpContext.Session.SetString("token", token);
                    var storedToken = HttpContext.Session.GetString("token");
                    if (string.IsNullOrEmpty(storedToken))
                    {
                        _logger.LogWarning("Token is not stored in session");
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt.");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            _logger.LogInformation("Model State is not valid or Captcha is not matched");
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
