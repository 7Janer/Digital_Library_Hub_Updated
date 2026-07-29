using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMSystem.Models;

public class LibraryContext : IdentityDbContext<ApplicationUser>
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Book> Books13 => Set<Book>();
    public DbSet<BorrowRecord> BorrowRecords13 => Set<BorrowRecord>();
    public DbSet<StudentModel> Students => Set<StudentModel>();
    public DbSet<LibrarianModel> Librarians => Set<LibrarianModel>();
    public DbSet<Publication> Publications => Set<Publication>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>().ToTable("Books13");
        modelBuilder.Entity<BorrowRecord>().ToTable("BorrowRecords13");

        modelBuilder.Entity<Book>()
            .HasMany(b => b.BorrowRecords)
            .WithOne(br => br.Book)
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Book>().HasData(
            new Book { BookId = 1, Title = "The Pragmatic Programmer", Author = "Andrew Hunt and David Thomas", ISBN = "978-0201616224", Publisher = "Addison-Wesley", Category = "Programming", PublishedDate = new DateTime(2021, 10, 30), IsAvailable = false },
            new Book { BookId = 2, Title = "Design Patterns using C#", Author = "Robert C. Martin", ISBN = "978-0132350884", Publisher = "Prentice Hall", Category = "Software Design", PublishedDate = new DateTime(2023, 8, 1), IsAvailable = true },
            new Book { BookId = 3, Title = "Mastering ASP.NET Core", Author = "Pranaya Kumar Rout", ISBN = "978-0451616235", Publisher = "TechPress", Category = "Web Development", PublishedDate = new DateTime(2022, 11, 22), IsAvailable = true },
            new Book { BookId = 4, Title = "SQL Server with DBA", Author = "Rakesh Kumar", ISBN = "978-4562350123", Publisher = "DataWorks", Category = "Database", PublishedDate = new DateTime(2020, 8, 15), IsAvailable = true },
            new Book { BookId = 5, Title = "Clean Code", Author = "Robert C. Martin", ISBN = "978-0132350884", Publisher = "Prentice Hall", Category = "Programming", PublishedDate = new DateTime(2008, 8, 1), IsAvailable = true },
            new Book { BookId = 6, Title = "The Hobbit", Author = "J. R. R. Tolkien", ISBN = "978-0547928227", Publisher = "Mariner Books", Category = "Fantasy", PublishedDate = new DateTime(2012, 9, 18), IsAvailable = true },
            new Book { BookId = 7, Title = "War and Peace", Author = "Leo Tolstoy", ISBN = "978-1400079988", Publisher = "Vintage", Category = "Classic", PublishedDate = new DateTime(2008, 12, 2), IsAvailable = true },
            new Book { BookId = 8, Title = "Computer Networks", Author = "Andrew S. Tanenbaum", ISBN = "978-0132126953", Publisher = "Pearson", Category = "Networking", PublishedDate = new DateTime(2010, 10, 7), IsAvailable = true },
            new Book { BookId = 9, Title = "Operating System Concepts", Author = "Abraham Silberschatz", ISBN = "978-1119800361", Publisher = "Wiley", Category = "Operating Systems", PublishedDate = new DateTime(2021, 12, 22), IsAvailable = true },
            new Book { BookId = 10, Title = "Introduction to Algorithms", Author = "Thomas H. Cormen", ISBN = "978-0262046305", Publisher = "MIT Press", Category = "Algorithms", PublishedDate = new DateTime(2022, 4, 5), IsAvailable = true },
            new Book { BookId = 11, Title = "Artificial Intelligence", Author = "Stuart Russell", ISBN = "978-0134610993", Publisher = "Pearson", Category = "AI", PublishedDate = new DateTime(2020, 4, 28), IsAvailable = true },
            new Book { BookId = 12, Title = "Cloud Computing", Author = "Rajkumar Buyya", ISBN = "978-0128128107", Publisher = "Morgan Kaufmann", Category = "Cloud", PublishedDate = new DateTime(2018, 12, 21), IsAvailable = true });

        modelBuilder.Entity<BorrowRecord>().HasData(
            new BorrowRecord
            {
                BorrowRecordId = 1,
                BookId = 1,
                BorrowerName = "Demo Member",
                BorrowerEmail = "member@example.com",
                Phone = "555-0301",
                BorrowDate = new DateTime(2026, 7, 20, 10, 0, 0, DateTimeKind.Utc),
                DueDate = new DateTime(2026, 8, 3, 10, 0, 0, DateTimeKind.Utc),
                ReturnDate = null
            });

        modelBuilder.Entity<StudentModel>().HasData(
            new StudentModel { StudentId = 1, StudentName = "Alice Johnson", Email = "alice.j@email.com", Phone = "555-0101" },
            new StudentModel { StudentId = 2, StudentName = "Bob Smith", Email = "bob.smith@email.com", Phone = "555-0102" },
            new StudentModel { StudentId = 3, StudentName = "Charlie Brown", Email = "charlie.b@email.com", Phone = "555-0103" },
            new StudentModel { StudentId = 4, StudentName = "Diana Prince", Email = "diana.p@email.com", Phone = "555-0104" },
            new StudentModel { StudentId = 5, StudentName = "Evan Wright", Email = "evan.w@email.com", Phone = "555-0105" },
            new StudentModel { StudentId = 6, StudentName = "Farah Khan", Email = "farah.k@email.com", Phone = "555-0106" },
            new StudentModel { StudentId = 7, StudentName = "George Miller", Email = "george.m@email.com", Phone = "555-0107" },
            new StudentModel { StudentId = 8, StudentName = "Hina Verma", Email = "hina.v@email.com", Phone = "555-0108" },
            new StudentModel { StudentId = 9, StudentName = "Ishaan Roy", Email = "ishaan.r@email.com", Phone = "555-0109" },
            new StudentModel { StudentId = 10, StudentName = "Julia Adams", Email = "julia.a@email.com", Phone = "555-0110" });

        modelBuilder.Entity<LibrarianModel>().HasData(
            new LibrarianModel { LibrarianId = 1, Name = "Sarah Connor", Age = 34, Phone = "555-0201" },
            new LibrarianModel { LibrarianId = 2, Name = "John Doe", Age = 28, Phone = "555-0202" },
            new LibrarianModel { LibrarianId = 3, Name = "Michael Scott", Age = 45, Phone = "555-0203" },
            new LibrarianModel { LibrarianId = 4, Name = "Ellen Ripley", Age = 39, Phone = "555-0204" },
            new LibrarianModel { LibrarianId = 5, Name = "James Bond", Age = 40, Phone = "555-0205" },
            new LibrarianModel { LibrarianId = 6, Name = "Neha Sharma", Age = 31, Phone = "555-0206" },
            new LibrarianModel { LibrarianId = 7, Name = "Omar Farooq", Age = 36, Phone = "555-0207" },
            new LibrarianModel { LibrarianId = 8, Name = "Priya Mehta", Age = 29, Phone = "555-0208" });

        modelBuilder.Entity<Publication>().HasData(
            new Publication { Id = 1, Title = "The Daily Times", Publisher = "Global Media Group", PublishedDate = new DateTime(2026, 7, 22), Type = PublicationType.Newspaper, IsAvailable = true },
            new Publication { Id = 2, Title = "Financial Chronicle", Publisher = "Wall Street Press", PublishedDate = new DateTime(2026, 7, 21), Type = PublicationType.Newspaper, IsAvailable = true },
            new Publication { Id = 3, Title = "Tech Weekly News", Publisher = "Silicon Valley Pubs", PublishedDate = new DateTime(2026, 7, 20), Type = PublicationType.Newspaper, IsAvailable = true },
            new Publication { Id = 4, Title = "Metro Morning Post", Publisher = "City Press House", PublishedDate = new DateTime(2026, 7, 22), Type = PublicationType.Newspaper, IsAvailable = true },
            new Publication { Id = 5, Title = "Saturday Sports Herald", Publisher = "Global Media Group", PublishedDate = new DateTime(2026, 7, 18), Type = PublicationType.Newspaper, IsAvailable = false },
            new Publication { Id = 6, Title = "National Geographic Vol 45", Publisher = "NatGeo Society", PublishedDate = new DateTime(2026, 7, 1), Type = PublicationType.Magazine, IsAvailable = true },
            new Publication { Id = 7, Title = "Vogue Fashion Summer", Publisher = "Conde Nast", PublishedDate = new DateTime(2026, 6, 15), Type = PublicationType.Magazine, IsAvailable = true },
            new Publication { Id = 8, Title = "Forbes Business 30 Under 30", Publisher = "Forbes Media", PublishedDate = new DateTime(2026, 7, 10), Type = PublicationType.Magazine, IsAvailable = false },
            new Publication { Id = 9, Title = "PC Gamer Ultimate", Publisher = "Future US", PublishedDate = new DateTime(2026, 7, 5), Type = PublicationType.Magazine, IsAvailable = true },
            new Publication { Id = 10, Title = "Scientific American", Publisher = "Springer Nature", PublishedDate = new DateTime(2026, 6, 28), Type = PublicationType.Magazine, IsAvailable = true },
            new Publication { Id = 11, Title = "India Today", Publisher = "Living Media", PublishedDate = new DateTime(2026, 7, 12), Type = PublicationType.Magazine, IsAvailable = true },
            new Publication { Id = 12, Title = "The Economic Herald", Publisher = "Economic Press", PublishedDate = new DateTime(2026, 7, 23), Type = PublicationType.Newspaper, IsAvailable = true });
    }
}
