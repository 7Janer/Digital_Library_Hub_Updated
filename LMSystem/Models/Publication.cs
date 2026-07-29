using System.ComponentModel.DataAnnotations;

namespace LMSystem.Models;

public enum PublicationType
{
    Newspaper = 0,
    Magazine = 1
}

public class Publication
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string? Title { get; set; }

    [Required, StringLength(80)]
    public string? Publisher { get; set; }

    [Required, DataType(DataType.Date)]
    [Display(Name = "Published Date")]
    public DateTime PublishedDate { get; set; }

    [Required]
    public PublicationType Type { get; set; }

    [Display(Name = "Available")]
    public bool IsAvailable { get; set; } = true;
}
