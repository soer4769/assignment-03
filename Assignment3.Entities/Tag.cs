namespace Assignment3.Entities;

public class Tag
{
    public int Id {get; set;}

    public string Name = null!;
    
    public IEnumerable<Task> Tasks = null!;
}
