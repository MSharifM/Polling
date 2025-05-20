using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Polling.Core.DTOs.User;
using Polling.Core.Genarator;
using Polling.Core.Sequrity;
using Polling.Core.Services.Interfaces;
using Polling.Datalayer.Entities;
using System.Security.Claims;

namespace Polling.Controllers.Account
{
    public class AccountController : Controller
    {
        private IUserServices _userServices;

        public AccountController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        #region Login

        [Route("/Login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                ViewBag.IsAuthenticated = true;

            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [Route("/Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model , string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            ViewData["returnUrl"] = returnUrl;

            var user = await _userServices.LoginUser(model);

            if (user != null)
            {
                //ToDo activate account by sms
                //if (user.IsActive)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier , user.UserId.ToString()),
                        new Claim(ClaimTypes.Name , user.FullName)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };
                    HttpContext.SignInAsync(principal, properties);

                    ViewBag.IsSuccess = true;
                    if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    if (user.IsAdmin)
                        return Redirect("Admin");
                    else
                        return Redirect("UserPanel");
                }
                //else
                //{
                //    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد.");
                //}
            }

            ViewBag.Error = "کد دانشجویی یا رمز عبور اشتباه است.";
            return View();
        }

        #endregion

        #region Logout

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        #endregion

    }
}
