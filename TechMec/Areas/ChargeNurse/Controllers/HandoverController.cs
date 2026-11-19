using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.ChargeNurse.Controllers
{
    [Area("ChargeNurse")]
    public class HandoverController : Controller
    {
        public IActionResult Index() => View();
    }
}