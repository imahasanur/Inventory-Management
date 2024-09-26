using Autofac;
using InventoryManagement.Data.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Web;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace InventoryManagement.Presentation.Models
{
    public class RegistrationModel
    {
        private ILifetimeScope _scope;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private LinkGenerator _linkGenerator;
        private IHttpContextAccessor _httpContextAccessor;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name ="Your Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage ="The {0} must be at least {2} and at max {1} characters long for pass.",MinimumLength =6)]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password",ErrorMessage ="Type properly! It's a mismatch with password")]
        public string ConfirmPassword { get; set; }

        public string? ReturnUrl { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string CaptchaInput { get; set; }


        public RegistrationModel() { }

        public RegistrationModel(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _userManager = _scope.Resolve<UserManager<ApplicationUser>>();
            _signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
            _linkGenerator = _scope.Resolve<LinkGenerator>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();

        }

        internal async Task<(IEnumerable<IdentityError>? errors, string? redirectLocation)> RegisterAsync(string urlPrefix)
        {
            ReturnUrl ??= urlPrefix;

            var user = new ApplicationUser { UserName = Email, Email = Email, FirstName = FirstName,
                LastName = LastName, FullName = $"{FirstName} {LastName}",
                CreatedAtUtc =new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };
            var result = await _userManager.CreateAsync(user, Password);
            
            if (result.Succeeded)
            {

				await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("User", "true"));
                await _signInManager.SignInAsync(user, isPersistent: false);
                return (null, ReturnUrl);
            }
            else
            {
                return (result.Errors, null);
            }
        }
    }
}
