using ManchesterUnitedApp.Data.Entities;
using ManchesterUnitedApp.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ManchesterUnitedApp.Services
{
    public class AdminService
    {
        private readonly UserManager<User> _userManager;

        public AdminService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // This method is not used by the controller but is kept for context.
        public async Task<IList<User>> GetAllNonSuperAdminsAsync()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var filteredUsers = new List<User>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("SuperAdmin"))
                {
                    filteredUsers.Add(user);
                }
            }
            return filteredUsers;
        }

        // FIX: Replaced the flawed GetByRoleAsync with a single, comprehensive method
        // that fetches ALL non-SuperAdmins and includes their Admin status.
        public async Task<IList<ApplicationUserModel>> GetAllNonSuperAdminsWithRolesAsync()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var userModels = new List<ApplicationUserModel>();

            foreach (var user in allUsers)
            {
                // 1. Skip the primary SuperAdmin account
                if (await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                {
                    continue;
                }

                // 2. Check if the user is an Admin (this will be used to sort them)
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                userModels.Add(new ApplicationUserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    ProfileImage = user.ProfileImage,
                    // Pass the role status back to the controller
                    IsAdmin = isAdmin
                });
            }
            return userModels;
        }

        // FIX 3: Clean up role determination and toggle logic.
        public async Task<bool> ChangeUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // Determine the current highest role for toggling
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            // Toggles the user between "Admin" and "User"
            string currentRole = isAdmin ? "Admin" : "User";
            string updatedRoleName = isAdmin ? "User" : "Admin";

            // Remove user from the current primary role
            var removeResult = await _userManager.RemoveFromRoleAsync(user, currentRole);
            // Ignore if the user wasn't actually in the role we tried to remove,
            // but for robust logging, you might check this. Here we proceed if removed or not found.
            // if (!removeResult.Succeeded) return false; 

            // Add user to the new role
            var addResult = await _userManager.AddToRoleAsync(user, updatedRoleName);
            return addResult.Succeeded;
        }

        // FIX 4: Simplified role check (prioritizes Admin, otherwise defaults to User for management)
        public async Task<string?> GetUserRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            // Explicitly check for Admin role first
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return "Admin";
            }

            // Otherwise, assume the standard User role for management display
            return "User";
        }
    }
}