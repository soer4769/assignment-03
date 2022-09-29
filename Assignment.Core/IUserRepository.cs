namespace Assignment.Core;

public interface IUserRepository
{
    (Response Response, int UserId) Create(UserCreateDTO user);
    UserDTO Find(int userId);
    IReadOnlyCollection<UserDTO> Read();
    Response Update(UserUpdateDTO user);
    Response Delete(int userId, bool force = false);
}