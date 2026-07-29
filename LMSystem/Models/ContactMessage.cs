using System.ComponentModel.DataAnnotations;

namespace LMSystem.Models;

public class ContactMessage
{
    public int ContactMessageId { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Full Name")]
    public string? Name { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required, StringLength(1200)]
    public string? Message { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public bool IsResolved { get; set; }
}
