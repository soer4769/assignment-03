namespace Assignment.Core;

public record UserDTO(int Id, string Name, string Email);

public record UserCreateDTO([StringLength(50)]string Name, [EmailAddress, StringLength(50)]string Email);

public record UserUpdateDTO(int Id, [StringLength(50)]string Name, [EmailAddress, StringLength(50)]string Email);