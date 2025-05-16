using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polling.Core.DTOs.User;
using Polling.Core.Services.Interfaces;

namespace Polling.Controllers.UserPanel
{
    [Authorize]
    public class UserPanelController : Controller
    {
        IUserServices _userServices;

        public UserPanelController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        #region Account

        public async Task<IActionResult> Index()
        {
            var user = await _userServices.GetUserInformationForUserPanel(User.Identity.Name);
            return View(user);
        }

        public async Task<IActionResult> EditAvatar()
        {
            var user = await _userServices.GetUserInformationForEditAvatar(User.Identity.Name);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditAvatar(EditAvatarViewModel model)
        {
            if (model.Avatar == null)
                return Redirect("/UserPanel");

            await _userServices.EditAvatar(model, User.Identity.Name);
            return Redirect("/UserPanel");
        }

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _userServices.EditPassword(User.Identity.Name, model))
                return Redirect("/UserPanel");

            ModelState.AddModelError("Error", "عملیات موفق نبود.");
            return View(model);
        }

        #endregion

        public async Task<IActionResult> ListPolls(int pageId = 1, string? filter = null, string getType = "all", string orderByType = "date")
        {
            var result = await _userServices.GetPollsToShowForUser(await _userServices.GetUserGroup(User.Identity.Name), pageId, filter
                , getType, orderByType, 8);

            return View(result);
        }

        public async Task<IActionResult> ShowPolls(int voteId)
        {
            return View();
        }
    }
}
