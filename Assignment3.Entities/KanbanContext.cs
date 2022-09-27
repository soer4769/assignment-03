using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Assignment3.Entities;

public class KanbanContext : DbContext
{
    public virtual DbSet<User> Users => Set<User>();

    public virtual DbSet<Tag> Tags => Set<Tag>();

    public virtual DbSet<Task> Tasks => Set<Task>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Name)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Task>()
            .Property(t => t.Title)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Task>()
            .Property(t => t.State)
            .IsRequired();

        modelBuilder.Entity<Tag>()
            .Property(t => t.Name)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();
    }

}
