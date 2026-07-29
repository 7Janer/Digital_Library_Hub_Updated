using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LMSystem.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(100)]
    [Display(Name = "Full Name")]
    public string? FullName { get; set; }

    public DateTime JoinedOn { get; set; } = DateTime.UtcNow;
}
