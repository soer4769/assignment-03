using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Infrastructure;

public class User
{
    public int Id { set; get; }
    
    public string Name { get; set; }
    
    public string Email { get; set; } = null!;

    public IEnumerable<WorkItem> Tasks { get; set; } = new List<WorkItem>();

}
