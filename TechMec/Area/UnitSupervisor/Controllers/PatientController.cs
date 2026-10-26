using Microsoft.AspNetCore.Mvc;


namespace TechMec.Area.UnitSupervisor.Controllers
{
    public class PatientController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}