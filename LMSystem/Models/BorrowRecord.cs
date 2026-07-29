using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSystem.Models;

public class BorrowRecord
{
    [Key]
    public int BorrowRecordId { get; set; }

    [Required]
    public int BookId { get; set; }

    [Required, StringLength(100)]
    public string? BorrowerName { get; set; }

    [Required, EmailAddress]
    public string? BorrowerEmail { get; set; }

    [Required, Phone]
    public string? Phone { get; set; }

    [BindNever]
    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

    [BindNever]
    public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(14);

    public DateTime? ReturnDate { get; set; }

    [BindNever]
    public Book? Book { get; set; }

    [NotMapped]
    public bool IsOverdue => ReturnDate is null && DateTime.UtcNow.Date > DueDate.Date;

    [NotMapped]
    public decimal EstimatedFine => Math.Max(0, (decimal)((ReturnDate ?? DateTime.UtcNow).Date - DueDate.Date).TotalDays) * 1.00m;
}
