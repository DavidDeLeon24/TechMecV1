using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.SystenAdmin.Controller
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
