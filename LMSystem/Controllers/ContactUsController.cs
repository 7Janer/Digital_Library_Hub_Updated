using LMSystem.Models;
using LMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSystem.Controllers;

public class ContactUsController : Controller
{
    private readonly LibraryContext _context;
    public ContactUsController(LibraryContext context) => _context = context;

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Index() => View(new ContactMessage());

    [AllowAnonymous]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactMessage model)
    {
        if (!ModelState.IsValid) return View(model);
        model.SubmittedAt = DateTime.UtcNow;
        _context.ContactMessages.Add(model);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Your support ticket has been submitted.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Tickets(string? searchTerm, int page = 1)
    {
        const int pageSize = 5;
        page = Math.Max(1, page);
        var query = _context.ContactMessages.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm)) query = query.Where(m => (m.Name != null && m.Name.Contains(searchTerm)) || (m.Email != null && m.Email.Contains(searchTerm)) || (m.Message != null && m.Message.Contains(searchTerm)));
        var total = await query.CountAsync();
        var totalPages = total == 0 ? 0 : (int)Math.Ceiling((double)total / pageSize);
        if (totalPages > 0) page = Math.Min(page, totalPages);
        var items = await query.OrderByDescending(m => m.SubmittedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return View(new PagedResult<ContactMessage> { Items = items, SearchTerm = searchTerm, CurrentPage = page, PageSize = pageSize, TotalItems = total });
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Resolve(int id)
    {
        var ticket = await _context.ContactMessages.FindAsync(id);
        if (ticket is not null) { ticket.IsResolved = true; await _context.SaveChangesAsync(); TempData["SuccessMessage"] = "Ticket marked as resolved."; }
        return RedirectToAction(nameof(Tickets));
    }
}
