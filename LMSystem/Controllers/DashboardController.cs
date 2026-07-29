using LMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSystem.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly LibraryContext _context;
    public DashboardController(LibraryContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var model = new DashboardModel
        {
            TotalStudents = await _context.Students.CountAsync(),
            TotalBooks = await _context.Books13.CountAsync(),
            TotalLibrarians = await _context.Librarians.CountAsync(),
            TotalBorrowings = await _context.BorrowRecords13.CountAsync(),
            ActiveBorrowings = await _context.BorrowRecords13.CountAsync(b => b.ReturnDate == null),
            TotalPublications = await _context.Publications.CountAsync(),
            TotalUsers = await _context.Users.CountAsync(),
            RecentBorrowings = await _context.BorrowRecords13.Include(b => b.Book).AsNoTracking().OrderByDescending(b => b.BorrowDate).Take(5).ToListAsync()
        };
        return View(model);
    }
}
