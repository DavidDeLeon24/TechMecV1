using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.UnitSupervisor.Controllers
{
    [Area("UnitSupervisor")]
    public class HandoverController : Controller
    {
        public IActionResult Index() => View();
    }
}