using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechMec.Models;
using TechMec.ViewModels;

namespace TechMec.Area.SystemAdmin.Controllers
{
    [Area("SystemAdmin")]
    [Authorize(Roles = "SystemAdmin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /SystemAdmin/User
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var displayUsers = new List<(ApplicationUser User, string Role)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "No Role";

                if (role != "SystemAdmin")
                {
                    displayUsers.Add((user, role));
                }
            }

            return View(displayUsers);
        }

        // GET: /SystemAdmin/User/Create
        public async Task<IActionResult> Create()
        {
            await PopulateRolesDropDown();
            return View();
        }

        // POST: /SystemAdmin/User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    Department = model.Department,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    TempData["Success"] = $"User {user.FullName} created successfully.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            await PopulateRolesDropDown(model.Role);
            return View(model);
        }

        // GET: /SystemAdmin/User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                FullName = user.FullName,
                Department = user.Department,
                Role = roles.FirstOrDefault() ?? "",
                IsActive = user.IsActive
            };

            await PopulateRolesDropDown(model.Role);
            return View(model);
        }

        // POST: /SystemAdmin/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound();

                if (user.Id == _userManager.GetUserId(User) && !model.IsActive)
                {
                    ModelState.AddModelError("IsActive", "You cannot deactivate your own account.");
                    await PopulateRolesDropDown(model.Role);
                    return View(model);
                }

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FullName = model.FullName;
                user.Department = model.Department;
                user.IsActive = model.IsActive;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                        ModelState.AddModelError("", error.Description);

                    await PopulateRolesDropDown(model.Role);
                    return View(model);
                }

                var currentRoles = await _userManager.GetRolesAsync(user);
                var currentRole = currentRoles.FirstOrDefault();

                if (currentRole != model.Role)
                {
                    if (currentRole != null)
                        await _userManager.RemoveFromRoleAsync(user, currentRole);

                    await _userManager.AddToRoleAsync(user, model.Role);
                }

                TempData["Success"] = $"User {user.FullName} updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateRolesDropDown(model.Role);
            return View(model);
        }

        // GET: /SystemAdmin/User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: /SystemAdmin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction(nameof(Index));
            }

            user.IsActive = false;
            await _userManager.UpdateAsync(user);

            TempData["Success"] = $"User {user.FullName} has been deactivated.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateRolesDropDown(string? selectedRole = null)
        {
            var allowedRoles = new List<string> { "FloorNurse", "ChargeNurse", "UnitSupervisor" };

            foreach (var r in allowedRoles)
            {
                if (!await _roleManager.RoleExistsAsync(r))
                    await _roleManager.CreateAsync(new IdentityRole(r));
            }

            ViewBag.Roles = new SelectList(allowedRoles, selectedRole);
        }
    }
}