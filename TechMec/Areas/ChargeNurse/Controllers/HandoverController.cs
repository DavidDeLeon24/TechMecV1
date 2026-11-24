using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMec.Data;
using TechMec.Models;

namespace TechMec.Area.ChargeNurse.Controllers
{
    [Area("ChargeNurse")]
    [Authorize(Roles = "ChargeNurse,SystemAdmin")] // Secure the area
    public class HandoverController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HandoverController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /ChargeNurse/Handover
        public async Task<IActionResult> Index()
        {
            var handovers = await _context.Handovers
                .Include(h => h.Patient)
                .Include(h => h.FromUser)
                .Include(h => h.ToUser)
                .OrderByDescending(h => h.HandoverTime)
                .ToListAsync();

            return View(handovers);
        }

        // GET: /ChargeNurse/Handover/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var handover = await _context.Handovers
                .Include(h => h.Patient)
                .Include(h => h.FromUser)
                .Include(h => h.ToUser)
                .FirstOrDefaultAsync(m => m.HandoverId == id);

            if (handover == null) return NotFound();

            // FETCH THE TASKS FOR THIS HANDOVER
            var tasks = await _context.TaskItems
                .Where(t => t.HandoverId == id)
                .ToListAsync();

            ViewBag.Tasks = tasks; // Pass them to the view

            return View(handover);
        }
    }
}