using Microsoft.AspNetCore.Mvc;

namespace TechMec.Area.ChargeNurse.Controllers
{
    [Area("ChargeNurse")]
    public class NotesController : Controller
    {
        public IActionResult Index() => View();
    }
}