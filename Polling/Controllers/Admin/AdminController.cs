using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Polling.Core.DTOs.Vote;
using Polling.Core.Services.Interfaces;

namespace Polling.Controllers.Admin
{
    public class AdminController : Controller
    {
        private IVoteService _voteService;

        public AdminController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateVote()
        {
            ViewData["selectGroups"] = await _voteService.GetAllGroups();
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVote(CreateVoteViewModel model , List<int> selectGroups)
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
            var voteInfo = await _voteService.GetVoteInformation(voteId);
            if (voteInfo == null)
                return NotFound();

            ViewData["TitleVote"] = voteInfo.Item1;
            ViewData["TextVote"] = voteInfo.Item2;
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

    }
}
