using Microsoft.AspNetCore.Mvc;
using Polling.Core.DTOs.User;
using Polling.Core.Genarator;
using Polling.Core.Sequrity;
using Polling.Core.Services.Interfaces;
using Polling.Datalayer.Entities;

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
                UserName = model.UserName,
                Email = model.Email,
                Phone = model.Phone,
                Password = PasswordHelper.EncodePasswordMd5(model.Password),
                ActiveCode = NameGenerator.GenerateUniqCode(),
                IsActive = false,
                IsDelete = false,
                UserAvatar = "Default.jpg",
                RegisterDate = DateTime.Now
            };

           _userServices.AddUser(user);

            return RedirectToAction("Login");
        }
        #endregion


        [Route("/Login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }
    }
}
