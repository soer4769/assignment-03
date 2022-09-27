namespace Assignment3.Entities;

public class UserRepository : IUserRepository
{
    private readonly KanbanContext _context;

    public UserRepository(KanbanContext context)
    {
        _context = context;
    }
    
    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        Response response;
        var entity = new User();
        entity.Email = user.Email;
        entity.Name = user.Name;

        _context.Users.Add(entity);
        _context.SaveChanges();

        response = Response.Created;


        return (response, entity.Id); 
    }

    public Response Delete(int userId, bool force = false)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if(user == null)
        {
            return Response.NotFound;
        } 
        _context.Users.Remove(user!);
        return Response.Deleted;
    }

    public UserDTO? Read(int userId)
    {
        var ReadUser = _context.Users.FirstOrDefault(u => u.Id == userId);
        return ReadUser == null ? null : new UserDTO(ReadUser.Id, ReadUser.Name, ReadUser.Email);
    }

    public IReadOnlyCollection<UserDTO> ReadAll()
    {
        var users = from u in _context.Users
                    select new UserDTO(u.Id, u.Name, u.Email);

        return users.ToArray();
    }

    public Response Update(UserUpdateDTO user)
    {
        var entity = _context.Users.Find(user.Id);
        if(entity == null) return Response.NotFound;
        entity.Name = user.Name;
        entity.Email = user.Email;
        _context.SaveChanges();

        return Response.Updated;

    }
}
