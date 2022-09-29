namespace Assignment.Core;

public record TagDTO(int Id, string Name);

public record TagCreateDTO([StringLength(50)]string Name);

public record TagUpdateDTO(int Id, [StringLength(50)]string Name);