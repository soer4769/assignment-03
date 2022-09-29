namespace Assignment.Infrastructure;

public class Tag
{
    public int Id {get; set;}

    public string Name = null!;
    
    public IEnumerable<WorkItem> Tasks = null!;
}
