using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.ChargeNurse.Controllers
{
    [Area("ChargeNurse")]
    public class TaskController : Controller
    {
        public IActionResult Index() => View();
    }
}