using LMSystem.Models;
using LMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSystem.Controllers;

[Authorize(Roles = "Administrator")]
public class LibrarianController : Controller
{
    private readonly LibraryContext _context;
    public LibrarianController(LibraryContext context) => _context = context;

    public async Task<IActionResult> Index(string? searchTerm, int page = 1)
    {
        const int pageSize = 5;
        page = Math.Max(1, page);
        var query = _context.Librarians.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();
            query = query.Where(l => (l.Name != null && l.Name.Contains(searchTerm)) || (l.Phone != null && l.Phone.Contains(searchTerm)));
        }
        var total = await query.CountAsync();
        var totalPages = total == 0 ? 0 : (int)Math.Ceiling((double)total / pageSize);
        if (totalPages > 0) page = Math.Min(page, totalPages);
        var items = await query.OrderBy(l => l.LibrarianId).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return View(new PagedResult<LibrarianModel> { Items = items, SearchTerm = searchTerm, CurrentPage = page, PageSize = pageSize, TotalItems = total });
    }

    [HttpGet] public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LibrarianModel model)
    {
        if (!ModelState.IsValid) return View(model);
        _context.Librarians.Add(model);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Librarian added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.Librarians.FindAsync(id);
        if (item is null) { TempData["ErrorMessage"] = $"No librarian found with ID {id}."; return View("NotFound"); }
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(LibrarianModel model)
    {
        if (!ModelState.IsValid) return View(model);
        _context.Librarians.Update(model);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Librarian updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Librarians.FindAsync(id);
        if (item is not null) { _context.Librarians.Remove(item); await _context.SaveChangesAsync(); TempData["SuccessMessage"] = "Librarian deleted successfully."; }
        return RedirectToAction(nameof(Index));
    }
}
