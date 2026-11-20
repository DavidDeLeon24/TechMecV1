
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMec.Data;
using TechMec.Models;

namespace TechMec.Area.ChargeNurse.Controllers
{
    [Area("ChargeNurse")]
    public class HandoverController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HandoverController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var handovers = await _context.Handovers
                .Include(h => h.Patient)
                .Include(h => h.FromUser)
                .Include(h => h.ToUser)
                .ToListAsync();

            return View(handovers);
        }
    }
}
