using Microsoft.AspNetCore.Mvc;
using Polling.Core.DTOs.Admin;
using Polling.Core.DTOs.User;
using Polling.Core.DTOs.Vote;
using Polling.Core.Sequrity;
using Polling.Core.Services.Interfaces;
using Polling.Datalayer.Entities;

namespace Polling.Controllers.Admin
{
    [PermissionChecker()]
    public class AdminController : Controller
    {
        private IVoteService _voteService;
        private IAdminService _adminService;

        public AdminController(IVoteService voteService, IAdminService adminService)
        {
            _voteService = voteService;
            _adminService = adminService;
        }

        public async Task<IActionResult> Index(int pageId = 1, string? filter = null, string getType = "all", string orderByType = "date")
        {
            var result = await _voteService.GetPollsToShowForUser(pageId, filter, getType, orderByType, 8);

            return View(result);
        }

        #region Polling

        #region Create New Vote

        public async Task<IActionResult> CreateVote()
        {
            ViewData["selectGroups"] = await _voteService.GetAllGroups();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVote(CreateVoteViewModel model, List<int> selectGroups)
        {
            if (!ModelState.IsValid)
            {
                ViewData["selectGroups"] = await _voteService.GetAllGroups();
                return View(model);
            }

            int id = await _voteService.AddVote(model, selectGroups);
            return RedirectToAction("AddOption", new { voteId = id });
        }

        public async Task<IActionResult> AddOption(int voteId)
        {
            var vote = await _voteService.GetVoteById(voteId);
            if (vote == null)
                return NotFound();

            ViewData["TitleVote"] = vote.Title;
            ViewData["TextVote"] = vote.Text;
            TempData["voteId"] = voteId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOption(List<CreateOptionViewModel> Options)
        {
            int id = (int)TempData["voteId"];

            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddOption", new { voteId = id });
            }

            await _voteService.AddOptions(id, Options);
            return Redirect("Index");
        }

        #endregion

        public async Task<IActionResult> ShowPoll(int voteId)
        {
            var vote = await _voteService.GetVoteDetailsForAdmin(voteId);
            if (vote == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Delete = false;

            return View(vote);
        }

        public async Task<IActionResult> DeletePoll(int voteId)
        {
            var vote = await _voteService.GetVoteDetailsForAdmin(voteId);
            if (vote == null)
            {
                return RedirectToAction("Index");
            }
            TempData["voteId"] = voteId;
            ViewBag.Delete = true;

            return View("ShowPoll", vote);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePoll()
        {
            int voteId = (int)TempData["voteid"];

            var vote = await _voteService.GetVoteById(voteId);
            if (vote == null)
            {
                return RedirectToAction("Index");
            }

            await _voteService.DeleteVote(voteId);
            return RedirectToAction("Index");
        }

        #endregion

        #region Users management

        [HttpGet]
        public async Task<IActionResult> ListUsers(int pageId = 1, string? filter = null)
        {
            var result = await _adminService.GetUsers(pageId, filter, 8);

            return View(result);
        }

        public async Task<IActionResult> AddUser()
        {
            ViewData["selectGroups"] = await _voteService.GetAllGroups();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(RegisterViewModel model, int selectedGroup)
        {
            if (!ModelState.IsValid)
            {
                ViewData["selectGroups"] = await _voteService.GetAllGroups();
                return View(model);
            }
            model.GroupId = selectedGroup;
            await _adminService.AddUser(model);
            return RedirectToAction("ListUsers");
        }

        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _adminService.GetUserById(id);
            if (user == null)
            {
                return RedirectToAction("ListUsers");
            }
            var model = new EditUserViewModel()
            {
                FullName = user.FullName,
                Phone = user.Phone,
                StudentCode = user.StudentCode,
                GroupId = user.GroupId,
                IsAdmin = user.IsAdmin,
            };
            TempData["userId"] = id;
            ViewData["Groups"] = await _voteService.GetAllGroups();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model , int selectedGroup, bool isadmin)
        {
            model.IsAdmin = isadmin;
            if (!ModelState.IsValid)
            {
                ViewData["Groups"] = await _voteService.GetAllGroups();
                return View(model);
            }
            int userId = (int)TempData["userId"];
            model.GroupId = selectedGroup;
            await _adminService.EditUser(model , userId);
            return RedirectToAction("ListUsers");
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _adminService.GetUserById(id);
            if (user == null)
            {
                return RedirectToAction("ListUsers");
            }
            TempData["userId"] = id;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser()
        {
            int userId = (int)TempData["userId"];
            await _adminService.DeleteUser(userId);
            return RedirectToAction("ListUsers");
        }

        #endregion

        #region Groups management

        public async Task<IActionResult> ListGroups()
        {
            var groups = await _voteService.GetAllGroups();
            return View(groups);
        }

        public async Task<IActionResult> AddGroup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return View();
            }
            await _adminService.AddGroup(groupName);
            return RedirectToAction("ListGroups");
        }

        public async Task<IActionResult> EditGroup(int id)
        {
            var model = await _adminService.GetGroupById(id);
            TempData["GroupId"] = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditGroup(Group model)
        {
            model.GroupId = (int)TempData["GroupId"];
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditGroup");
            }
            await _adminService.EditGroup(model);
            return RedirectToAction("ListGroups");
        }

        public async Task<IActionResult> DeleteGroup(int id)
        {
            await _adminService.DeleteGroup(id);
            return RedirectToAction("ListGroups");
        }
        #endregion

    }
}
