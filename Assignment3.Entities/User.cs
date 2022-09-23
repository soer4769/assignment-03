using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities;

public class User
{
    public int Id { set; get; }
    
    public string Name { get; set; }
    
    public string Email { get; set; } = null!;

    public IEnumerable<Task> Tasks { get; set; } = null!;

}
