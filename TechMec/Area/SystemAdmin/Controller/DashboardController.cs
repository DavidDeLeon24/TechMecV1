using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.SystenAdmin.Controller
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
