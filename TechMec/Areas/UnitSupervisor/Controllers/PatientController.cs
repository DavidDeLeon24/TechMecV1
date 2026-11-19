using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMec.Data;

namespace TechMec.Area.UnitSupervisor.Controllers
{
    [Area("UnitSupervisor")]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var patients = _context.Patients.ToList();
            return View(patients);
        }
    }
}