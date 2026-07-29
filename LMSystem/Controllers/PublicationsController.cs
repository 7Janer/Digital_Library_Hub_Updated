using LMSystem.Models;
using LMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSystem.Controllers;

[Authorize]
public class PublicationsController : Controller
{
    private readonly LibraryContext _context;
    public PublicationsController(LibraryContext context) => _context = context;

    public async Task<IActionResult> Index(PublicationType? type, string? searchTerm, int page = 1)
    {
        const int pageSize = 5;
        page = Math.Max(1, page);
        var query = _context.Publications.AsNoTracking().AsQueryable();
        if (type.HasValue) query = query.Where(p => p.Type == type.Value);
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();
            query = query.Where(p => (p.Title != null && p.Title.Contains(searchTerm)) || (p.Publisher != null && p.Publisher.Contains(searchTerm)));
        }
        var total = await query.CountAsync();
        var totalPages = total == 0 ? 0 : (int)Math.Ceiling((double)total / pageSize);
        if (totalPages > 0) page = Math.Min(page, totalPages);
        var items = await query.OrderByDescending(p => p.PublishedDate).ThenBy(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return View(new PublicationIndexViewModel { Items = items, Type = type, SearchTerm = searchTerm, CurrentPage = page, PageSize = pageSize, TotalItems = total });
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpGet]
    public IActionResult Create(PublicationType? type) => View(new Publication { Type = type ?? PublicationType.Newspaper, PublishedDate = DateTime.Today, IsAvailable = true });

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Publication model)
    {
        if (!ModelState.IsValid) return View(model);
        _context.Publications.Add(model);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"{model.Type} added successfully.";
        return RedirectToAction(nameof(Index), new { type = model.Type });
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.Publications.FindAsync(id);
        if (item is null) { TempData["ErrorMessage"] = "Publication not found."; return View("NotFound"); }
        return View(item);
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Publication model)
    {
        if (!ModelState.IsValid) return View(model);
        _context.Publications.Update(model);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"{model.Type} updated successfully.";
        return RedirectToAction(nameof(Index), new { type = model.Type });
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Publications.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (item is null) { TempData["ErrorMessage"] = "Publication not found."; return View("NotFound"); }
        return View(item);
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.Publications.FindAsync(id);
        if (item is null) { TempData["ErrorMessage"] = "Publication not found."; return View("NotFound"); }
        var type = item.Type;
        _context.Publications.Remove(item);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"{type} deleted successfully.";
        return RedirectToAction(nameof(Index), new { type });
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleAvailability(int id)
    {
        var item = await _context.Publications.FindAsync(id);
        if (item is not null)
        {
            item.IsAvailable = !item.IsAvailable;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Publication availability updated.";
            return RedirectToAction(nameof(Index), new { type = item.Type });
        }
        return RedirectToAction(nameof(Index));
    }
}
