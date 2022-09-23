namespace Assignment3.Entities;

public class Tag
{
    public int Id {get; set;}

    [Required]
    [StringLength(50)]
    public string Name = null!;
    

    public IEnumerable<Task> Tasks = null!;
}
