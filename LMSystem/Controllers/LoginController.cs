using LMSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.Controllers;

// Compatibility controller for older links used in the original project PDFs.
public class LoginController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    public LoginController(SignInManager<ApplicationUser> signInManager) => _signInManager = signInManager;

    public IActionResult Index() => RedirectToAction("Login", "Account");

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
}
