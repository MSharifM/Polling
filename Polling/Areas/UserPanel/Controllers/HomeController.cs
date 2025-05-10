using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polling.Core.DTOs.User;
using Polling.Core.Services.Interfaces;

namespace Polling.Areas.UserPanel.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IUserServices _userServices;

        public HomeController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [Area("UserPanel")]
        public async Task<IActionResult> Index()
        {
            var user = await _userServices.GetUserInformationForUserPanel(User.Identity.Name);
            return View(user);
        }

        [Route("UserPanel/EditAvatar/")]
        public async Task<IActionResult> EditAvatar()
        {
            var user = await _userServices.GetUserInformationForEditAvatar(User.Identity.Name);
            return View("/Areas/UserPanel/Views/Home/EditAvatar.cshtml" , user);
        }

        [HttpPost]
        [Route("UserPanel/EditAvatar")]
        public async Task<IActionResult> EditAvatar(EditAvatarViewModel model)
        {
            if (model.Avatar == null)
                return Redirect("/UserPanel");

            await _userServices.EditAvatar(model, User.Identity.Name);
            return Redirect("/UserPanel");
        }

        [Route("UserPanel/ChangePassword/")]
        public async Task<IActionResult> ChangePassword()
        {
            return View("/Areas/UserPanel/Views/Home/ChangePassword.cshtml");
        }

        [HttpPost]
        [Route("UserPanel/ChangePassword/")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View("/Areas/UserPanel/Views/Home/ChangePassword.cshtml" , model);

            if (await _userServices.EditPassword(User.Identity.Name, model))
                return Redirect("/UserPanel");

            ModelState.AddModelError("Error", "عملیات موفق نبود.");
            return View("/Areas/UserPanel/Views/Home/ChangePassword.cshtml", model);
        }

        public async Task<IActionResult> ChangeEmail()
        {
            return View();
        }
    }
}
