namespace LMSystem.Models;

public class DashboardModel
{
    public int TotalStudents { get; set; }
    public int TotalBooks { get; set; }
    public int TotalLibrarians { get; set; }
    public int TotalBorrowings { get; set; }
    public int ActiveBorrowings { get; set; }
    public int TotalPublications { get; set; }
    public int TotalUsers { get; set; }
    public List<BorrowRecord> RecentBorrowings { get; set; } = new();
}
