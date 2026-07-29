using System.Diagnostics;
using LMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSystem.Controllers;

public class HomeController : Controller
{
    private readonly LibraryContext _context;
    public HomeController(LibraryContext context) => _context = context;

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        ViewBag.FeaturedBooks = await _context.Books13.AsNoTracking().OrderBy(b => b.BookId).Take(3).ToListAsync();
        ViewBag.TotalBooks = await _context.Books13.CountAsync();
        ViewBag.TotalMembers = await _context.Students.CountAsync();
        ViewBag.TotalPublications = await _context.Publications.CountAsync();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
