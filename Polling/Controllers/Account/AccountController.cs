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

        #region Register 

        [HttpGet]
        [Route("/Register")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user = new User()
            {
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                StudentCode = model.StudentCode,
                Password = PasswordHelper.EncodePasswordMd5(model.Password),
                ActiveCode = NameGenerator.GenerateUniqCode(),
                IsActive = false,
                IsDelete = false,
                UserAvatar = "Default.jpg",
                RegisterDate = DateTime.Now
            };

           await _userServices.AddUser(user);

            return RedirectToAction("Login");
        }

        #endregion

        #region Login

        [Route("/Login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
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
                    return Redirect("Home");
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
