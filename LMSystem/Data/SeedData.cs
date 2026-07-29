using LMSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace LMSystem.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = ["Administrator", "Librarian", "Member"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await EnsureUserAsync(userManager, "admin@example.com", "admin@example.com", "System Administrator", "Password123", "Administrator");
        await EnsureUserAsync(userManager, "librarian@example.com", "librarian@example.com", "Demo Librarian", "Password123", "Librarian");
        await EnsureUserAsync(userManager, "member@example.com", "member@example.com", "Demo Member", "Password123", "Member");
        await EnsureUserAsync(userManager, "admin", "legacyadmin@library.local", "Legacy Administrator", "12345", "Administrator");
    }

    private static async Task EnsureUserAsync(UserManager<ApplicationUser> userManager, string username, string email, string fullName, string password, string role)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true,
                JoinedOn = DateTime.UtcNow
            };
            var created = await userManager.CreateAsync(user, password);
            if (!created.Succeeded)
            {
                throw new InvalidOperationException(string.Join("; ", created.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
