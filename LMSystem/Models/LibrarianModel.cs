using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSystem.Models;

[Table("Librarians")]
public class LibrarianModel
{
    [Key]
    public int LibrarianId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Range(18, 100, ErrorMessage = "Age must be between 18 and 100.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Phone is required.")]
    [Phone]
    public string? Phone { get; set; }
}
