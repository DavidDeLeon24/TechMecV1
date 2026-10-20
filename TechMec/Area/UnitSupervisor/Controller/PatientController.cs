using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.UnitSupervisor.Controller
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
