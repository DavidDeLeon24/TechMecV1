using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechMec.Data;
using TechMec.Models;

namespace TechMec.Area.ChargeNurse.Controllers
{
    [Area("ChargeNurse")] // THIS IS CRITICAL
    [Authorize(Roles = "ChargeNurse,SystemAdmin")]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
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
            // We must ignore validation for the Patient navigation property
            // because we only want to save the ID.
            ModelState.Remove("Patient");

            if (ModelState.IsValid)
            {
                note.CreatedAt = DateTime.UtcNow;
                _context.Notes.Add(note);
                await _context.SaveChangesAsync();

                // Redirect to a success page or back to Dashboard
                return RedirectToAction("Index", "Dashboard");
            }
            return View(note);
        }
    }
}