namespace Assignment3.Entities;
using System.ComponentModel.DataAnnotations;

public enum State {New, Active, Resolved, Closed, Removed}

public class Task
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title
    { get; set; } = null!;
    
    [Required]
    public State State { get; set; }
    
    public User? AssignedTo { get; set; }
    
    public string? Description { get; set; }

    public IEnumerable<Tag> Tags 
    { get; set; } = null!;
}
