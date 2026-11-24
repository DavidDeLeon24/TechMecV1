using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechMec.Data;
using TechMec.Models;
using System.ComponentModel.DataAnnotations;

namespace TechMec.Area.UnitSupervisor.Controllers
{
    [Area("UnitSupervisor")]
    [Authorize(Roles = "UnitSupervisor,SystemAdmin")]
    public class HandoverController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HandoverController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Form fields for creating a handover
        public class CreateHandoverInput
        {
            [Range(1, int.MaxValue, ErrorMessage = "Please select a patient.")]
            public int PatientId { get; set; }

            [Required(ErrorMessage = "Please select a shift.")]
            public HandoverShift? ShiftType { get; set; }

            [Required(ErrorMessage = "Enter the From Nurse name.")]
            public string FromNurseName { get; set; } = "";

            [Required(ErrorMessage = "Enter the To Nurse name.")]
            public string ToNurseName { get; set; } = "";

            [StringLength(2000)]
            public string? SpecialInstructions { get; set; }
        }

        private async Task LoadPatients()
        {
            var patients = await _context.Patients
                .OrderBy(p => p.FirstName)
                .Select(p => new
                {
                    p.PatientId,
                    Name = p.FirstName + " " + p.LastName
                })
                .ToListAsync();

            ViewBag.Patients = new SelectList(patients, "PatientId", "Name");
        }

        // GET: /UnitSupervisor/Handover
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

        // GET: /UnitSupervisor/Handover/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadPatients();
            return View(new CreateHandoverInput());
        }

        // POST: /UnitSupervisor/Handover/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateHandoverInput input)
        {
            if (ModelState.IsValid)
            {
                bool patientExists = await _context.Patients.AnyAsync(p => p.PatientId == input.PatientId);
                if (!patientExists)
                {
                    ModelState.AddModelError("PatientId", "Please select a valid patient.");
                }

                string fromName = input.FromNurseName.Trim();
                string toName = input.ToNurseName.Trim();

                var fromMatches = await _context.Users.Where(u => u.FullName == fromName).Take(2).ToListAsync();
                var toMatches = await _context.Users.Where(u => u.FullName == toName).Take(2).ToListAsync();

                if (fromMatches.Count != 1)
                {
                    ModelState.AddModelError("FromNurseName", fromMatches.Count == 0 ? "From Nurse was not found." : "Multiple users have this name.");
                }

                if (toMatches.Count != 1)
                {
                    ModelState.AddModelError("ToNurseName", toMatches.Count == 0 ? "To Nurse was not found." : "Multiple users have this name.");
                }

                if (ModelState.IsValid)
                {
                    var handover = new Handover
                    {
                        PatientId = input.PatientId,
                        ShiftType = input.ShiftType!.Value,
                        FromUserId = fromMatches[0].Id,
                        ToUserId = toMatches[0].Id,
                        SpecialInstructions = input.SpecialInstructions
                    };

                    try
                    {
                        _context.Handovers.Add(handover);
                        await _context.SaveChangesAsync();
                        TempData["Success"] = "Handover created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException ex)
                    {
                        ModelState.AddModelError("", "Unable to save handover: " + (ex.InnerException?.Message ?? ex.Message));
                    }
                }
            }

            await LoadPatients();
            return View(input);
        }

        // GET: /UnitSupervisor/Handover/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var handover = await _context.Handovers
                .Include(h => h.Patient)
                .Include(h => h.FromUser)
                .Include(h => h.ToUser)
                .FirstOrDefaultAsync(m => m.HandoverId == id);

            if (handover == null) return NotFound();

            return View(handover);
        }

        // GET: /UnitSupervisor/Handover/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var handover = await _context.Handovers.FindAsync(id);
            if (handover == null) return NotFound();

            return View(handover);
        }

        // POST: /UnitSupervisor/Handover/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Handover handover)
        {
            if (id != handover.HandoverId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(handover);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Handover updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Handovers.Any(e => e.HandoverId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(handover);
        }

        // GET: /UnitSupervisor/Handover/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var handover = await _context.Handovers
                .Include(h => h.Patient)
                .Include(h => h.FromUser)
                .Include(h => h.ToUser)
                .FirstOrDefaultAsync(m => m.HandoverId == id);

            if (handover == null) return NotFound();

            return View(handover);
        }

        // POST: /UnitSupervisor/Handover/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var handover = await _context.Handovers.FindAsync(id);
            if (handover != null)
            {
                _context.Handovers.Remove(handover);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Handover deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}