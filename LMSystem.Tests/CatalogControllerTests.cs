using FluentAssertions;
using LMSystem.Controllers;
using LMSystem.Models;
using LMSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LMSystem.Tests;

public class CatalogControllerTests : IDisposable
{
    private readonly LibraryContext _context;

    public CatalogControllerTests()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new LibraryContext(options);
        Seed();
    }

    [Fact]
    public async Task BooksIndex_FiltersByTitleAuthorOrIsbn()
    {
        var controller = new BooksController(_context);
        var result = await controller.Index("node", 1);
        var view = result.Should().BeOfType<ViewResult>().Subject;
        var model = view.Model.Should().BeOfType<PagedResult<Book>>().Subject;
        model.Items.Should().ContainSingle();
        model.Items[0].Title.Should().Be("Node.js in Action");
    }

    [Fact]
    public async Task BooksIndex_PaginatesFiveRecordsPerPage()
    {
        var controller = new BooksController(_context);
        var result = await controller.Index(null, 2);
        var model = result.Should().BeOfType<ViewResult>().Subject.Model.Should().BeOfType<PagedResult<Book>>().Subject;
        model.Items.Should().HaveCount(2);
        model.TotalPages.Should().Be(2);
        model.CurrentPage.Should().Be(2);
    }

    [Fact]
    public async Task PublicationsIndex_FiltersByPublicationType()
    {
        var controller = new PublicationsController(_context);
        var result = await controller.Index(PublicationType.Magazine, null, 1);
        var model = result.Should().BeOfType<ViewResult>().Subject.Model.Should().BeOfType<PublicationIndexViewModel>().Subject;
        model.Items.Should().OnlyContain(p => p.Type == PublicationType.Magazine);
    }

    private void Seed()
    {
        for (var i = 1; i <= 6; i++)
        {
            _context.Books13.Add(new Book
            {
                BookId = i,
                Title = i == 6 ? "Node.js in Action" : $"Book {i}",
                Author = "Test Author",
                ISBN = $"978-{i:0000000000}",
                PublishedDate = new DateTime(2024, 1, Math.Min(i, 28)),
                IsAvailable = true
            });
        }
        _context.Books13.Add(new Book
        {
            BookId = 7,
            Title = "Cloud Patterns",
            Author = "Another Author",
            ISBN = "978-9999999999",
            PublishedDate = new DateTime(2024, 2, 1),
            IsAvailable = true
        });
        _context.Publications.AddRange(
            new Publication { Id = 1, Title = "Daily News", Publisher = "Press", PublishedDate = DateTime.Today, Type = PublicationType.Newspaper },
            new Publication { Id = 2, Title = "Science Monthly", Publisher = "Lab", PublishedDate = DateTime.Today, Type = PublicationType.Magazine });
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
