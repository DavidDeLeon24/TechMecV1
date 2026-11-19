using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.SystemAdmin.Controllers
{
    [Area("SystemAdmin")]
    public class DashboardController : Controller
    {
        public IActionResult Index() => View();
    }
}