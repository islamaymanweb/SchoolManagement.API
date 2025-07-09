using Microsoft.AspNetCore.Identity;
using SchoolManagement.API.Data;
using SchoolManagement.API.Models.Entities;

namespace SchoolManagement.Auth;

public static class RoleInitializer
{
    public static async Task CreateRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        var config = serviceProvider.GetRequiredService<IConfiguration>();

        string[] roleNames = { "Administrator", "Student", "Teacher" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var email = config["AdminAccount:Email"]
            ?? throw new ArgumentNullException("AdminAccount:Email");

        var password = config["AdminAccount:Password"]
            ?? throw new ArgumentNullException("AdminAccount:Password");

        var firstName = config["AdminAccount:FirstName"]
            ?? throw new ArgumentNullException("AdminAccount:FirstName");

        var lastName = config["AdminAccount:LastName"]
            ?? throw new ArgumentNullException("AdminAccount:LastName");

        var adminUser = await userManager.FindByEmailAsync(email);

        if (adminUser == null)
        {
            var newAdmin = new User
            {
                UserName = email,
                Email = email,
                IsActive = true,
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            var result = await userManager.CreateAsync(newAdmin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Administrator");

                var adminEntity = new Admin
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserId = newAdmin.Id
                };

                dbContext.Admins.Add(adminEntity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}