using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace LMSystem.Models;

public class Book
{
    [BindNever]
    public int BookId { get; set; }

    [Required(ErrorMessage = "The Title field is required.")]
    [StringLength(100)]
    public string? Title { get; set; }

    [Required(ErrorMessage = "The Author field is required.")]
    [StringLength(100)]
    public string? Author { get; set; }

    [Required(ErrorMessage = "The ISBN field is required.")]
    [RegularExpression(@"^\d{3}-\d{10}$", ErrorMessage = "ISBN must be in the format XXX-XXXXXXXXXX.")]
    public string? ISBN { get; set; }

    [StringLength(100)]
    public string? Publisher { get; set; }

    [StringLength(60)]
    public string? Category { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Published Date")]
    public DateTime PublishedDate { get; set; }

    [BindNever]
    [Display(Name = "Available")]
    public bool IsAvailable { get; set; } = true;

    [BindNever]
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}
