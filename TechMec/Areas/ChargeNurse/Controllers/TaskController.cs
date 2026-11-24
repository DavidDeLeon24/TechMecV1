using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechMec.Data;
using TechMec.Models;

namespace TechMec.Area.ChargeNurse.Controllers
{
    [Area("ChargeNurse")]
    [Authorize(Roles = "ChargeNurse,SystemAdmin")]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /ChargeNurse/Task
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.TaskItems
                .Include(t => t.Handover)
                .ToListAsync();
            return View(tasks);
        }

        // GET: /ChargeNurse/Task/Create
        public async Task<IActionResult> Create()
        {
            // Populate the dropdown with existing Handovers
            var handovers = await _context.Handovers
                .Select(h => new {
                    h.HandoverId,
                    Display = "Handover ID: " + h.HandoverId + " - Patient: " + h.Patient.FirstName
                })
                .ToListAsync();

            ViewBag.HandoverId = new SelectList(handovers, "HandoverId", "Display");
            return View();
        }

        // POST: /ChargeNurse/Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            // Remove navigation property validation
            ModelState.Remove("Handover");
            ModelState.Remove("CompletedByUserId");

            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Task created successfully.";
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload the dropdown
            var handovers = await _context.Handovers
                .Select(h => new {
                    h.HandoverId,
                    Display = "Handover ID: " + h.HandoverId + " - Patient: " + h.Patient.FirstName
                })
                .ToListAsync();
            ViewBag.HandoverId = new SelectList(handovers, "HandoverId", "Display", task.HandoverId);

            return View(task);
        }

        // GET: /ChargeNurse/Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return NotFound();

            return View(task);
        }

        // POST: /ChargeNurse/Task/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem task)
        {
            if (id != task.TaskId) return NotFound();

            ModelState.Remove("Handover");

            if (ModelState.IsValid)
            {
                try
                {
                    // Automatically record the completion time if marked as complete
                    if (task.IsCompleted && task.CompletedAt == null)
                    {
                        task.CompletedAt = DateTime.UtcNow;
                    }

                    _context.Update(task);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Task updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TaskItems.Any(e => e.TaskId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }
    }
}