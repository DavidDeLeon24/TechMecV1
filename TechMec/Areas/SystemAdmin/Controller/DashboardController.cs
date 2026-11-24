using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechMec.Models;

namespace TechMec.Area.SystemAdmin.Controllers
{
    [Area("SystemAdmin")]
    [Authorize(Roles = "SystemAdmin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var nurseList = new List<(ApplicationUser User, string Role)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "No Role";

                if (role != "SystemAdmin")
                {
                    nurseList.Add((user, role));
                }
            }

            var sortedNurses = nurseList.OrderBy(n => n.User.FullName).ToList();
            return View(sortedNurses);
        }
    }
}