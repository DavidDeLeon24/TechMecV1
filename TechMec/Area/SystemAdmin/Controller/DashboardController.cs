using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.SystemAdmin
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}