using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polling.Core.Services.Interfaces;

namespace Polling.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        IUserServices _userServices;

        public HomeController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userServices.GetUserInformationByName(User.Identity.Name);
            return View(user);
        }

        public async Task<IActionResult> EditProfile()
        {
            return View();
        }

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        public async Task<IActionResult> ChangeEmail()
        {
            return View();
        }
    }
}
