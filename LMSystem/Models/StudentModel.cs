using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSystem.Models;

[Table("Students")]
public class StudentModel
{
    [Key]
    public int StudentId { get; set; }

    [Required(ErrorMessage = "Student name is required.")]
    [StringLength(100)]
    [Column("Student_Name")]
    [Display(Name = "Student Name")]
    public string? StudentName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Phone is required.")]
    [Phone]
    [Column("Phone_Number")]
    public string? Phone { get; set; }
}
