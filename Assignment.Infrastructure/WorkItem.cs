namespace Assignment.Infrastructure;
using System.ComponentModel.DataAnnotations;

public class WorkItem
{
    public int Id { get; set; }

    public string Title
    { get; set; } = null!;
    
    public State State { get; set; }
    
    public User? AssignedTo { get; set; }
    
    public string? Description { get; set; }

    public IReadOnlyCollection<Tag> Tags 
    { get; set; } = null!;
}
