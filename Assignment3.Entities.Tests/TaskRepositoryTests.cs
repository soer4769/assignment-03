global using Microsoft.Data.Sqlite;
global using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests()
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

        var task = new Task {Id = 1, Title = "Spaghetti", AssignedTo = context.Users.Find(1)};
        var task2 = new Task {Id = 2, Title = "Meatballs", AssignedTo = context.Users.Find(1)};

        context.Add(user);
        context.Add(task);
        context.Add(task2);
        context.SaveChanges();

        _context = context;
        _repository = new TaskRepository(_context);
    }
    

    [Fact]
    public void Create_should_return_task()
    {
        // Arrange
        var task = new TaskCreateDTO("Chocolate", null, null, new List<string>());

        // Act 
        var actual = _repository.Create(task);

        // Assert
        actual.Should()
            .Be((Response.Created, 3));
    }

    [Fact]
    public void ReadAll_should_return_smth()
    {
        var actual = _repository.ReadAll();
        
        actual.Should().BeEquivalentTo(new object[] {
            new TaskDTO(1, "Spaghetti", "Poul Poulsen", new List<string>(), State.New), new TaskDTO(2, "Meatballs", "Poul Poulsen", new List<string>(), State.New)
        });
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
