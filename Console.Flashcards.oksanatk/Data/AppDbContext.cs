using dotnetMAUI.Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetMAUI.Flashcards.Data;

public class AppDbContext : DbContext
{
    public DbSet<StudySession> StudySessions { get; set; }
    public DbSet<Stack> Stacks { get; set; }
    public DbSet<Flashcard> Flashcards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flashcard>()
            .HasOne(f => f.Stack)
            .WithMany(s => s.Flashcards)
            .HasForeignKey( f => f.StackId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudySession>()
            .HasOne(ss => ss.Stack)
            .WithMany(s => s.StudySessions)
            .HasForeignKey(ss => ss.StackId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Stack>()
            .HasIndex(s => s.Name)
            .IsUnique();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\FlashcardsProject;Database=FlashcardsDB;Integrated Security=True;");
    }
}
