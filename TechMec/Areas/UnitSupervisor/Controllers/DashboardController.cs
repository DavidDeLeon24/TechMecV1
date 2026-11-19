using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.UnitSupervisor.Controllers
{
    [Area("UnitSupervisor")]
    public class DashboardController : Controller
    {
        public IActionResult Index() => View();
    }
}