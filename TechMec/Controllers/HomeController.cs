using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechMec.Models;

namespace TechMec.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() => View();

        // Routes the signed-in user to their area dashboard
        public IActionResult Dashboard()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Dashboard", new { area = "SystemAdmin" });

            if (User.IsInRole("UnitSupervisor"))
                return RedirectToAction("Index", "Dashboard", new { area = "UnitSupervisor" });

            if (User.IsInRole("ChargeNurse"))
                return RedirectToAction("Index", "Dashboard", new { area = "ChargeNurse" });

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}