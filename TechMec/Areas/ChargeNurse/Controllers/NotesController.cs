using Microsoft.AspNetCore.Mvc;
using TechMec.Data;
using TechMec.Models;

namespace TechMec.Area.ChargeNurse.Controllers
{
    [Area("ChargeNurse")]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Notes/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Note note)
        {
            if (ModelState.IsValid)
            {
                note.CreatedAt = DateTime.UtcNow;
                _context.Notes.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Dashboard"); 
            }
            return View(note);
        }
    }
}