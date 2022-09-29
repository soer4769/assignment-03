global using Microsoft.Data.Sqlite;
global using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Assignment.Infrastructure.Tests;

public class WorkItemRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly WorkItemRepository _repository;

    public WorkItemRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        
        var user = new User {
            Id = 1,
            Name = "Poul Poulsen",
            Email = "poul@thepoul.dk"
        };

        var task = new WorkItem {Id = 1, Title = "Spaghetti", AssignedTo = context.Users.Find(1)};
        var task2 = new WorkItem {Id = 2, Title = "Meatballs", AssignedTo = context.Users.Find(1)};

        context.Add(user);
        context.Add(task);
        context.Add(task2);
        context.SaveChanges();

        _context = context;
        _repository = new WorkItemRepository(_context);
    }
    

    [Fact]
    public void Create_should_return_task()
    {
        // Arrange
        var task = new WorkItemCreateDTO("Chocolate", null, null, new List<string>());

        // Act 
        var actual = _repository.Create(task);

        // Assert
        actual.Should()
            .Be((Response.Created, 3));
    }

    [Fact]
    public void Read_should_return_smth()
    {
        var actual = _repository.Read();

        actual.ElementAt(0);
        //actual.ElementAt(0).Equals(new WorkItemDTO(1, "Spaghetti", "Poul Poulsen", new List<string>(), State.New));
        //actual.ElementAt(1).Equals(new WorkItemDTO(2, "Meatballs", "Poul Poulsen", new List<string>(), State.New));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}