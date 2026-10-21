using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechMec.Models;
using TechMec.ViewModels;

namespace TechMec.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;  //for role - aj

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole>roleManager) //added
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager; //added for role
        }

        [HttpGet] //shows login page
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            //checks db for a user with the enterd email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            //checks password
            var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            if (result.IsLockedOut)
                ModelState.AddModelError("", "Account locked. Try again in 15 minutes.");
            else
                ModelState.AddModelError("", "Invalid email or password");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}