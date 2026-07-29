using LMSystem.Models;
using LMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSystem.Controllers;

[Authorize(Roles = "Administrator")]
public class UserAdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public UserAdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index(string? searchTerm)
    {
        var users = await _userManager.Users.AsNoTracking().OrderBy(u => u.UserName).ToListAsync();
        if (!string.IsNullOrWhiteSpace(searchTerm)) users = users.Where(u => (u.UserName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) || (u.Email?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) || (u.FullName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
        var model = new List<UserAdminViewModel>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            model.Add(new UserAdminViewModel { User = user, Role = roles.FirstOrDefault() ?? "Member" });
        }
        ViewBag.SearchTerm = searchTerm;
        ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignRole(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null || !await _roleManager.RoleExistsAsync(role)) { TempData["ErrorMessage"] = "User or role not found."; return RedirectToAction(nameof(Index)); }
        var existing = await _userManager.GetRolesAsync(user);
        if (existing.Count > 0) await _userManager.RemoveFromRolesAsync(user, existing);
        await _userManager.AddToRoleAsync(user, role);
        TempData["SuccessMessage"] = $"{user.UserName} is now a {role}.";
        return RedirectToAction(nameof(Index));
    }
}
