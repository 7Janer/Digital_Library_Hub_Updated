using LMSystem.Models;
using LMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSystem.Controllers;

[Authorize(Roles = "Administrator,Librarian")]
public class StudentController : Controller
{
    private readonly LibraryContext _context;
    public StudentController(LibraryContext context) => _context = context;

    public async Task<IActionResult> Index(string? searchTerm, int page = 1)
    {
        const int pageSize = 5;
        page = Math.Max(1, page);
        var query = _context.Students.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();
            query = query.Where(s => (s.StudentName != null && s.StudentName.Contains(searchTerm)) || (s.Email != null && s.Email.Contains(searchTerm)) || (s.Phone != null && s.Phone.Contains(searchTerm)));
        }
        var total = await query.CountAsync();
        var totalPages = total == 0 ? 0 : (int)Math.Ceiling((double)total / pageSize);
        if (totalPages > 0) page = Math.Min(page, totalPages);
        var items = await query.OrderBy(s => s.StudentId).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return View(new PagedResult<StudentModel> { Items = items, SearchTerm = searchTerm, CurrentPage = page, PageSize = pageSize, TotalItems = total });
    }

    [HttpGet] public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentModel model)
    {
        if (!ModelState.IsValid) return View(model);
        _context.Students.Add(model);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Student added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.Students.FindAsync(id);
        if (item is null) { TempData["ErrorMessage"] = $"No student found with ID {id}."; return View("NotFound"); }
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(StudentModel model)
    {
        if (!ModelState.IsValid) return View(model);
        _context.Students.Update(model);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Student updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Students.FindAsync(id);
        if (item is not null) { _context.Students.Remove(item); await _context.SaveChangesAsync(); TempData["SuccessMessage"] = "Student deleted successfully."; }
        return RedirectToAction(nameof(Index));
    }
}
