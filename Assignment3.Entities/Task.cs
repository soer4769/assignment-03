using System.ComponentModel.DataAnnotations;

namespace Assignment3.Entities;

public enum State {New, Active, Resolved, Closed, Removed}

public class Task
{
    public int Id { get; set; }

    [StringLength(100)]
    public required string Title {
        get;
        set;
    }
    
    public required State state { get; set; }
    
    public User? AssignedTo { get; set; }
    
    public string? Description { get; set; }
    
    public IEnumerable<Tag> Tags { get; set; }
}
