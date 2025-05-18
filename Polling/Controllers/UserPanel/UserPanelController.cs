using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polling.Core.DTOs.User;
using Polling.Core.Services;
using Polling.Core.Services.Interfaces;

namespace Polling.Controllers.UserPanel
{
    [Authorize]
    public class UserPanelController : Controller
    {
        private IUserServices _userServices;
        private IVoteService _voteService;

        public UserPanelController(IUserServices userServices, IVoteService voteService)
        {
            _userServices = userServices;
            _voteService = voteService;
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
            var result = await _userServices.GetPollsToShowForUser(User.Identity.Name ,await _userServices.GetUserGroup(User.Identity.Name), pageId, filter
                , getType, orderByType, 8);

            return View(result);
        }

        public async Task<IActionResult> ShowPolls(int voteId)
        {
            var vote = await _voteService.GetVoteById(voteId);

            int userGroup = await _userServices.GetUserGroup(User.Identity.Name);
            if (vote.Groups.Any(g => g.GroupId == userGroup))
            {
                int userId = (await _userServices.GetUserByName(User.Identity.Name)).UserId;
                ViewBag.Participated = await UserService.IsUserParticipatedInVote(userId, vote.VoteId);
                TempData["VoteId"] = vote.VoteId;
                return View(vote);
            }
            else
            {
                return Redirect("ListPolls");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitVote(List<int>? selectedOptions, int? selectOption)
        {
            var user = await _userServices.GetUserByName(User.Identity.Name);

            if (selectedOptions.Count == 0 && selectOption == null)
            {
                ModelState.AddModelError("Error", "لطفا گزینه ای را انتخاب کنید.");
                return Redirect("ListPolls");

            }
            if (selectedOptions.Count != 0)
            {
                await _userServices.AddUserVote(user.UserId, (int)TempData["VoteId"], selectedOptions);
            }
            else
            {
                selectedOptions.Clear();
                selectedOptions.Add((int)selectOption);
                await _userServices.AddUserVote(user.UserId, (int)TempData["VoteId"], selectedOptions);
            }

            return Redirect("ListPolls");
        }

    }
}
