using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechMec.Data;
using TechMec.Models;

namespace TechMec.Controllers
{
    [Authorize] //must be logg in to have access to this
    public class HandoversController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HandoversController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //showing inbox for the nurse thats logged in
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);//gets ID of user thts currenctly looged in 

            //gets the handover data from database, "ToUserId" = current user for ex Nurse 1
            var myInbox = await _context.Handovers
                .Include(h => h.Patient)
                .Include(h => h.FromUser)
                .Where(h => h.ToUserId == userId)
                .OrderByDescending(h => h.HandoverTime)
                .ToListAsync();

            return View(myInbox); //shows the html page
        }

     //idk yet


    }
}