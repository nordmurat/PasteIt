using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasteIt.Models.ViewModels;

namespace PasteIt.Controllers
{
    public class AuthController : Controller
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly SignInManager<IdentityUser> _signInManager;
        readonly IConfiguration _config;

        public AuthController(UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signInManager, IConfiguration config)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
            _config = config;
        }

        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(User)) return RedirectToAction("Login");

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInViewModel appUserViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser appUser = new IdentityUser
                {
                    UserName = appUserViewModel.UserName,
                    Email = appUserViewModel.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, appUserViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            //TempData["returnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            //var returnUrl = TempData["returnUrl"]?.ToString() ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                await _signInManager.SignOutAsync();
                IdentityUser user = await _userManager.FindByNameAsync(model.UserName);
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.Persistent, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                    //return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset(UserResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.To.Add(user.Email);
                mail.From = new MailAddress("ikilikmurat@outlook.com", "Murat İkilik", System.Text.Encoding.UTF8);
                mail.Subject = "Password Reset";
                mail.Body = $"<a target=\"_blank\" href=\"https://localhost:7096{Url.Action("UpdatePassword", "Auth", new { userId = user.Id, token = HttpUtility.UrlEncode(resetToken) })}\">Change of password!</a>";
                mail.IsBodyHtml = true;
                SmtpClient smp = new SmtpClient();
                smp.Credentials = new NetworkCredential("ikilikmurat@outlook.com", "Halkciecevit006.");
                smp.Port = 587;
                smp.Host = "smtp-mail.outlook.com";
                smp.EnableSsl = true;
                smp.Send(mail);

                ViewBag.State = true;
            }
            else
            {
                ViewBag.State = false;
            }

            return View();
        }

        [HttpGet("[action]/{userId}/{token}")]
        public IActionResult UpdatePassword(string userId, string token)
        {
            return View();
        }

        [HttpPost("[action]/{userId}/{token}")]
        public async Task<IActionResult> UpdatePassword(UserUpdatePasswordViewModel model, string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(token), model.Password);
            if (result.Succeeded)
            {
                ViewBag.State = true;
                await _userManager.UpdateSecurityStampAsync(user);
            }
            else
                ViewBag.State = false;

            return View();
        }
    }
}