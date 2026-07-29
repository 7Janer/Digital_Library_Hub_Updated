using LMSystem.Models;

namespace LMSystem.ViewModels;

public class PublicationIndexViewModel : PagedResult<Publication>
{
    public PublicationType? Type { get; set; }
}
