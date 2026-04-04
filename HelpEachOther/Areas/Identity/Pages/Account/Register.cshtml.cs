using System.ComponentModel.DataAnnotations;
using HelpEachOther.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace HelpEachOther.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IOptions<IdentityOptions> identityOptions)
    {
        _userManager = userManager;
        _signInManager = signInManager;

        var passwordOptions = identityOptions.Value.Password;
        PasswordRules = new List<string>
        {
            $"Minimum length: {passwordOptions.RequiredLength} characters",
            passwordOptions.RequireUppercase ? "At least one uppercase letter (A-Z)" : "Uppercase letters are not required",
            passwordOptions.RequireLowercase ? "At least one lowercase letter (a-z)" : "Lowercase letters are not required",
            passwordOptions.RequireDigit ? "At least one number (0-9)" : "Numbers are not required",
            passwordOptions.RequireNonAlphanumeric ? "At least one special character (for example: !, @, #)" : "Special characters are not required"
        };
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public IList<string> PasswordRules { get; }

    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new ApplicationUser
        {
            UserName = Input.Email,
            Email = Input.Email
        };

        var result = await _userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}
