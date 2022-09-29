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
        context.Add(new User {
            Id = 1,
            Name = "Poul Poulsen",
            Email = "poul@thepoul.dk"
        });

        var tag1 = new Tag();
        tag1.Id = 0;
        tag1.Name = "eat cake";

        var task = new WorkItem {Id = 1, Title = "Spaghetti", AssignedTo = context.Users.Find(1), Tags = new List<Tag>{tag1}, State = State.New};
        var task2 = new WorkItem {Id = 2, Title = "Meatballs", AssignedTo = context.Users.Find(1), State = State.Removed};

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
        actual.Should().Be((Response.Created, 3));
    }

    [Fact]
    public void Read_returns_id_1_and_2()
    {
        var actual = _repository.Read();
        actual.Should().BeEquivalentTo(new[]{
            new WorkItemDTO(1, "Spaghetti", "Poul Poulsen", new List<string>{"eat cake"}.AsReadOnly(), State.New),
            new WorkItemDTO(2, "Meatballs", "Poul Poulsen", new List<string>().AsReadOnly(), State.Removed)
        });
    }

    [Fact]
    public void ReadRemoved_returns_id_2()
    {
        var actual = _repository.ReadRemoved();
        actual.Should().BeEquivalentTo(new[]{
            new WorkItemDTO(2, "Meatballs", "Poul Poulsen", new List<string>().AsReadOnly(), State.Removed)
        });
    }

    [Fact]
    public void ReadByUser_returns_id_1()
    {
        var actual = _repository.ReadByUser(1);
        actual.First().Id.Should().Be(1);
    }

    [Fact]
    public void ReadByState_returns_id_1()
    {
        var actual = _repository.ReadByState(State.New);
        actual.Should().BeEquivalentTo(new[]{
            new WorkItemDTO(1, "Spaghetti", "Poul Poulsen", new List<string>{"eat cake"}.AsReadOnly(), State.New)
        });
    }

    [Fact]
    public void ReadByTag_returns_id_1()
    {
        var actual = _repository.ReadByTag("eat cake");
        actual.First().Id.Should().Be(1);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
