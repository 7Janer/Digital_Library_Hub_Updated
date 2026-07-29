using LMSystem.Models;
using LMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSystem.Controllers;

[Authorize]
public class BooksController : Controller
{
    private readonly LibraryContext _context;
    public BooksController(LibraryContext context) => _context = context;

    public async Task<IActionResult> Index(string? searchTerm, int page = 1)
    {
        const int pageSize = 5;
        page = Math.Max(1, page);
        var query = _context.Books13.Include(b => b.BorrowRecords).AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();
            query = query.Where(b =>
                (b.Title != null && b.Title.Contains(searchTerm)) ||
                (b.Author != null && b.Author.Contains(searchTerm)) ||
                (b.ISBN != null && b.ISBN.Contains(searchTerm)) ||
                (b.Publisher != null && b.Publisher.Contains(searchTerm)) ||
                (b.Category != null && b.Category.Contains(searchTerm)));
        }
        var total = await query.CountAsync();
        var totalPages = total == 0 ? 0 : (int)Math.Ceiling((double)total / pageSize);
        if (totalPages > 0) page = Math.Min(page, totalPages);
        var items = await query.OrderBy(b => b.BookId).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return View(new PagedResult<Book> { Items = items, SearchTerm = searchTerm, CurrentPage = page, PageSize = pageSize, TotalItems = total });
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null or 0) return NotFoundView("Book ID was not provided.");
        var book = await _context.Books13.AsNoTracking().FirstOrDefaultAsync(b => b.BookId == id);
        return book is null ? NotFoundView($"No book found with ID {id}.") : View(book);
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpGet]
    public IActionResult Create() => View();

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book)
    {
        if (!ModelState.IsValid) return View(book);
        book.IsAvailable = true;
        _context.Books13.Add(book);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Successfully added {book.Title}.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null or 0) return NotFoundView("Book ID was not provided.");
        var book = await _context.Books13.AsNoTracking().FirstOrDefaultAsync(b => b.BookId == id);
        return book is null ? NotFoundView($"No book found with ID {id}.") : View(book);
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Book book)
    {
        if (!ModelState.IsValid) { book.BookId = id; return View(book); }
        var existing = await _context.Books13.FindAsync(id);
        if (existing is null) return NotFoundView($"No book found with ID {id}.");
        existing.Title = book.Title;
        existing.Author = book.Author;
        existing.ISBN = book.ISBN;
        existing.Publisher = book.Publisher;
        existing.Category = book.Category;
        existing.PublishedDate = book.PublishedDate;
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Successfully updated {book.Title}.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null or 0) return NotFoundView("Book ID was not provided.");
        var book = await _context.Books13.AsNoTracking().FirstOrDefaultAsync(b => b.BookId == id);
        return book is null ? NotFoundView($"No book found with ID {id}.") : View(book);
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books13.FindAsync(id);
        if (book is null) return NotFoundView($"No book found with ID {id}.");
        _context.Books13.Remove(book);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Successfully deleted {book.Title}.";
        return RedirectToAction(nameof(Index));
    }

    private ViewResult NotFoundView(string message)
    {
        TempData["ErrorMessage"] = message;
        return View("NotFound");
    }
}
