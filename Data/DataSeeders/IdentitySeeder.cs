using ManchesterUnitedApp.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace ManchesterUnitedApp.Data.DataSeeders
{
    public class IdentitySeeder
    {
        public static async Task SeedSuperAdminAsync
            (UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            #region Create roles
            if (!await roleManager.RoleExistsAsync("SuperAdmin"))
            {
                await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))//user role
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
            #endregion

            #region Create supperadmin user
            string email = "superadmin@mail.com";
            string password = "admin123";

            var superAdminUser = await userManager.FindByEmailAsync(email);
            if (superAdminUser == null)
            {
                superAdminUser = new User
                {
                    UserName = email,
                    Email = email
                };
                var result = await userManager.CreateAsync(superAdminUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                }
            }
            #endregion
        }
    }
}