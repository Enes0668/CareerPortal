using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    // Register GET
    [HttpGet]
    public IActionResult Register() => View();

    // Register POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email, // login email ile
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Email doğrulama token oluştur
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = user.Id, token }, Request.Scheme);

                // Email gönder
                await _emailSender.SendEmailAsync(user.Email, "Email Doğrulama",
                    $"Hesabınızı doğrulamak için <a href='{confirmationLink}'>buraya tıklayın</a>.");

                return View("RegistrationConfirmation"); // Email gönderildi sayfası
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    // ConfirmEmail GET
    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
            return RedirectToAction("Index", "Home");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound();

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
            return View("EmailConfirmed");
        else
            return View("Error");
    }

    // Login GET
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // Login POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Lütfen email adresinizi doğrulayın.");
                    return View(model);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                    // Claims ile FullName’i sakla
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    var identity = new ClaimsIdentity(claims, "login");
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity),
                        new Microsoft.AspNetCore.Authentication.AuthenticationProperties { IsPersistent = model.RememberMe });

                    return RedirectToLocal(returnUrl);
                }
            }

            ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
        }

        return View(model);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction("Index", "Home");
    }

    // Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            // Email gönderilmese bile aynı mesajı göster
            return View("ForgotPasswordConfirmation");
        }

        // Token oluştur
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = Url.Action("ResetPassword", "Account",
            new { token, email = user.Email }, Request.Scheme);

        await _emailSender.SendEmailAsync(user.Email, "Şifre Sıfırlama",
            $"Şifrenizi sıfırlamak için <a href='{resetLink}'>buraya tıklayın</a>.");

        return View("ForgotPasswordConfirmation");
    }
    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        if (token == null || email == null)
            return RedirectToAction("Index", "Home");

        var model = new ResetPasswordViewModel { Token = token, Email = email };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return RedirectToAction("ResetPasswordConfirmation");

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded)
            return RedirectToAction("ResetPasswordConfirmation");

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

}
