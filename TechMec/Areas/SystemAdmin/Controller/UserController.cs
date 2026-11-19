using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.SystemAdmin.Controllers
{
    [Area("SystemAdmin")]
    public class UserController : Controller
    {
        public IActionResult Index() => View();
    }
}