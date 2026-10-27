using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.SystemAdmin.Controllers
{
    [Area("SystemAdmin")]
    public class UserController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}