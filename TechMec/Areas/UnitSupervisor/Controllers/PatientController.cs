
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMec.Data;
using TechMec.Models;

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

        // Display all patients
        public async Task<IActionResult> Index()
        {
            var patients = await _context.Patients.ToListAsync();
            return View(patients);
        }

        // GET: Create Patient
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Save Patient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Patient Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // GET: Edit Patient
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // POST: Save Edited Patient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient patient)
        {
            if (id != patient.PatientId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(patient);

            var existingPatient =
                await _context.Patients.FindAsync(id);

            if (existingPatient == null)
                return NotFound();

            existingPatient.FirstName = patient.FirstName;
            existingPatient.LastName = patient.LastName;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.RoomNumber = patient.RoomNumber;
            existingPatient.Diagnosis = patient.Diagnosis;
            existingPatient.Status = patient.Status;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Delete Confirmation
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // POST: Delete Patient
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return NotFound();

            bool hasHandovers = await _context.Handovers
                .AnyAsync(h => h.PatientId == id);

            if (hasHandovers)
            {
                ModelState.AddModelError(
                    "",
                    "This patient has handovers and cannot be deleted."
                );

                return View("Delete", patient);
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
