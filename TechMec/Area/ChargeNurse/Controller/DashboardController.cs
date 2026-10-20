using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.ChargeNurse.Controller
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
