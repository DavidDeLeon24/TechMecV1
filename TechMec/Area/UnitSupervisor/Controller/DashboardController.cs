using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.UnitSupervisor.Controller
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
