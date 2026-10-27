using Microsoft.AspNetCore.Mvc;


namespace TechMec.Area.UnitSupervisor.Controllers
{
    public class DashboardController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}