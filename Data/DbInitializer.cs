using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BuildTracker.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BuildTrackerContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.Migrate();

                // Seed Roles
                string[] roleNames = { "Admin", "User" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Seed Admin User
                var adminUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@buildtracker.com",
                    EmailConfirmed = true
                };

                if (await userManager.FindByNameAsync("admin") == null)
                {
                    var result = await userManager.CreateAsync(adminUser, "Password123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
            }
        }
    }
}
