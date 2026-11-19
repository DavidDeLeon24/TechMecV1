using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.ChargeNurse.Controllers
{
    [Area("ChargeNurse")]
    public class DashboardController : Controller
    {
        public IActionResult Index() => View();
    }
}