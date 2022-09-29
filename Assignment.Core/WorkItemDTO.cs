namespace Assignment.Core;

public record WorkItemDTO(int Id, string Title, string AssignedToName, IReadOnlyCollection<string> Tags, State State);

public record WorkItemDetailsDTO(int Id, string Title, string Description, DateTime Created, string AssignedToName, IReadOnlyCollection<string> Tags, State State, DateTime StateUpdated);

public record WorkItemCreateDTO([StringLength(100)]string Title, int? AssignedToId, string? Description, ICollection<string> Tags);

public record WorkItemUpdateDTO(int Id, [StringLength(100)]string Title, int? AssignedToId, string? Description, ICollection<string> Tags, State State);