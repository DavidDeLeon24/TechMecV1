using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.ChargeNurse.Controllers
{
    public class TaskController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
