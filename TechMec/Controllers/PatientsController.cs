using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMec.Data;
using TechMec.Models;
namespace TechMec.Controllers
{
    [Authorize] //need to be logged in to see this controller // this is a list of patient records
    public class PatientsController : Controller
    {
        // connection to the database.
        private readonly ApplicationDbContext _context;

        // database connection
        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //this shows the list of all patients 
        public async Task<IActionResult> Index()
        {
            //from the database and sends it to the page 
            return View(await _context.Patients.ToListAsync());
        }

        //blank form 
        public IActionResult Create()
        {
            //empty form
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //validation check
        public async Task<IActionResult> Create(Patient patient) //saving new patient to db(takes info typed into the form)
        {
            // If form is filled out correctly (checks the patients data if its valid like if the FirstName is empty it will not work)
            if (ModelState.IsValid)
            {
                _context.Patients.Add(patient); // Add to DB
                await _context.SaveChangesAsync(); // Save to db
                return RedirectToAction(nameof(Index)); // Back to the patient list
            }

            //if invalid form, then it goes back to fix mistakes like empty textbox or too many characs
            return View(patient);
        }

        //shows form with existing data
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound(); // No ID provided

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound(); // Patient doesn't exist in the database

            return View(patient); // Pre-fill the form
        }

        // Save the updated info for existing patient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient patient)
        {
            // Make sure we're editing the right person (correct ID)
            if (id != patient.PatientId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Patients.Update(patient); // Updates DB with the new data 
                await _context.SaveChangesAsync(); // Save changes/updates
                return RedirectToAction(nameof(Index));// go back to list
            }
            return View(patient); // if invalid, then shows form again with errors
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) //removing patient
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient != null)
            {
                _context.Patients.Remove(patient); //remove from database
                await _context.SaveChangesAsync(); //saves and permanently deletes the patient from the database, shows error if patient has existing handovers though from ApplicationDbContext (DeleteBehaviour.Restrict)
            }
            return RedirectToAction(nameof(Index)); //back to the list
        }

    }
}