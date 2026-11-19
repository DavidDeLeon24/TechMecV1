using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMec.Data;
using TechMec.Models;

namespace TechMec.Controllers
{
    [Authorize]
    public class HandoversController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HandoversController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Inbox for the logged-in nurse
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var myInbox = await _context.Handovers
                .Include(h => h.Patient)
                .Include(h => h.FromUser)
                .Where(h => h.ToUserId == userId)
                .OrderByDescending(h => h.HandoverTime)
                .ToListAsync();

            return View(myInbox);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var handover = await _context.Handovers
                .Include(h => h.Patient)
                .Include(h => h.FromUser)
                .Include(h => h.ToUser)
                .FirstOrDefaultAsync(h => h.HandoverId == id);

            if (handover == null) return NotFound();

            return View(handover);
        }
    }
}