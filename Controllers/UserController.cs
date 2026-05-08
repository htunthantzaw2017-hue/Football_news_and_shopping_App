using ManchesterUnitedApp.Models.Users;
using ManchesterUnitedApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; // Ensure this is present for Task<IActionResult>

namespace ManchesterUnitedApp.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        private readonly AdminService _adminService;

        public UserController(AdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            // FIX: Use the single, fixed service method to get all non-SuperAdmin users
            var allNonSuperAdmins = await _adminService.GetAllNonSuperAdminsWithRolesAsync();

            var allUsers = new RoleGroupedUsersModel();

            // Loop through the single list and classify them locally based on the IsAdmin flag
            foreach (var user in allNonSuperAdmins)
            {
                // This sorting relies on the IsAdmin property set in ApplicationUserModel
                // and populated by AdminService.GetAllNonSuperAdminsWithRolesAsync()
                if (user.IsAdmin)
                {
                    allUsers.Admins.Add(user);
                }
                else
                {
                    allUsers.Users.Add(user);
                }
            }

            return View(allUsers);
        }

        public async Task<IActionResult> ChangeRole(string id)
        {
            // This method uses the corrected toggle logic in AdminService.cs
            bool isChangeRole = await _adminService.ChangeUserRoleAsync(id);

            return RedirectToAction("Index");
        }
    }
}