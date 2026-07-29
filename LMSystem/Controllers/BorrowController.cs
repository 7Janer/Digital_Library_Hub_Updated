using LMSystem.Models;
using LMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSystem.Controllers;

[Authorize]
public class BorrowController : Controller
{
    private readonly LibraryContext _context;
    public BorrowController(LibraryContext context) => _context = context;

    [Authorize(Roles = "Administrator,Librarian")]
    public async Task<IActionResult> Index(string? searchTerm, int page = 1)
    {
        const int pageSize = 5;
        page = Math.Max(1, page);
        var query = _context.BorrowRecords13.Include(r => r.Book).AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();
            query = query.Where(r => (r.BorrowerName != null && r.BorrowerName.Contains(searchTerm)) || (r.BorrowerEmail != null && r.BorrowerEmail.Contains(searchTerm)) || (r.Book != null && r.Book.Title != null && r.Book.Title.Contains(searchTerm)));
        }
        var total = await query.CountAsync();
        var totalPages = total == 0 ? 0 : (int)Math.Ceiling((double)total / pageSize);
        if (totalPages > 0) page = Math.Min(page, totalPages);
        var items = await query.OrderByDescending(r => r.BorrowDate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return View(new PagedResult<BorrowRecord> { Items = items, SearchTerm = searchTerm, CurrentPage = page, PageSize = pageSize, TotalItems = total });
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? bookId)
    {
        if (bookId is null or 0) return NotFoundView("Book ID was not provided for borrowing.");
        var book = await _context.Books13.FindAsync(bookId);
        if (book is null) return NotFoundView($"No book found with ID {bookId}.");
        if (!book.IsAvailable) { TempData["ErrorMessage"] = $"{book.Title} is currently unavailable."; return View("NotAvailable"); }
        var currentUser = User.Identity?.Name;
        return View(new BorrowViewModel { BookId = book.BookId, BookTitle = book.Title, BorrowerEmail = currentUser?.Contains('@') == true ? currentUser : null });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BorrowViewModel model)
    {
        var book = await _context.Books13.FindAsync(model.BookId);
        if (book is null) return NotFoundView("The selected book no longer exists.");
        model.BookTitle = book.Title;
        if (!ModelState.IsValid) return View(model);
        if (!book.IsAvailable) { TempData["ErrorMessage"] = $"{book.Title} has already been borrowed."; return View("NotAvailable"); }

        book.IsAvailable = false;
        _context.BorrowRecords13.Add(new BorrowRecord
        {
            BookId = book.BookId,
            BorrowerName = model.BorrowerName,
            BorrowerEmail = model.BorrowerEmail,
            Phone = model.Phone,
            BorrowDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14)
        });
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"{book.Title} borrowed successfully. It is due in 14 days.";
        return User.IsInRole("Member") ? RedirectToAction("Index", "Books") : RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpGet]
    public async Task<IActionResult> Return(int? borrowRecordId)
    {
        if (borrowRecordId is null or 0) return NotFoundView("Borrow record ID was not provided.");
        var record = await _context.BorrowRecords13.Include(r => r.Book).FirstOrDefaultAsync(r => r.BorrowRecordId == borrowRecordId);
        if (record is null) return NotFoundView("Borrow record not found.");
        if (record.ReturnDate is not null) { TempData["ErrorMessage"] = "This book has already been returned."; return View("AlreadyReturned"); }
        return View(new ReturnViewModel { BorrowRecordId = record.BorrowRecordId, BookTitle = record.Book?.Title, BorrowerName = record.BorrowerName, BorrowDate = record.BorrowDate });
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(ReturnViewModel model)
    {
        var record = await _context.BorrowRecords13.Include(r => r.Book).FirstOrDefaultAsync(r => r.BorrowRecordId == model.BorrowRecordId);
        if (record is null) return NotFoundView("Borrow record not found.");
        if (record.ReturnDate is not null) { TempData["ErrorMessage"] = "This book has already been returned."; return View("AlreadyReturned"); }
        record.ReturnDate = DateTime.UtcNow;
        if (record.Book is not null) record.Book.IsAvailable = true;
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"{record.Book?.Title} returned successfully.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrator,Librarian")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Extend(int id)
    {
        var record = await _context.BorrowRecords13.FindAsync(id);
        if (record is null) return NotFoundView("Borrow record not found.");
        if (record.ReturnDate is not null) { TempData["ErrorMessage"] = "A returned loan cannot be extended."; return RedirectToAction(nameof(Index)); }
        record.DueDate = record.DueDate.AddDays(7);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Due date extended by 7 days.";
        return RedirectToAction(nameof(Index));
    }

    private ViewResult NotFoundView(string message)
    {
        TempData["ErrorMessage"] = message;
        return View("NotFound");
    }
}
