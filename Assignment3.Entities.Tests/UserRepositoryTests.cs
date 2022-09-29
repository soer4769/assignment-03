namespace Assignment3.Entities.Tests;

public class UserRepositoryTests : IDisposable
{

    
    private readonly KanbanContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
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
        var user2 = new User {
            Id = 2,
            Name = "Jens Jensen",
            Email = "jens@thejensen.dk"
        };
        context.Add(user);
        context.Add(user2);
        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

    [Fact]
    public void Create_returns_userID_and_created_response() {
        //Arrange
        var user = new UserCreateDTO("Jens", "jens@jensen.dk");

        //act
        var actual = _repository.Create(user);
        //assert
        actual.Should().Be((Response.Created, 3));
    }

    [Fact]
    public void Delete_returns_deleted_response_given_userId() {
        

        var actual = _repository.Delete(1);

        actual.Should().Be(Response.Deleted);
    }

    [Fact]
    public void Read_returns_UserDTO_given_userId() {

        var actual = _repository.Read(1);

        var expected = new UserDTO(1, "Poul Poulsen", "poul@thepoul.dk");

        actual.Should().Be(expected);
    }

    [Fact]
    public void ReadAll_returns_all_users() {
        var actual = _repository.ReadAll();
        actual.Should().BeEquivalentTo(new[] {
            new UserDTO(1,"Poul Poulsen", "poul@thepoul.dk"), new UserDTO(2, "Jens Jensen", "jens@thejensen.dk")
        });
    }

    [Fact]
    public void Update_returns_response_given_user_update_dto() {
        var newUser = new UserUpdateDTO(1, "Poul Poulsen", "poulcool@thepoul.dk");
        var actual = _repository.Update(newUser);

        actual.Should().Be(Response.Updated);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
