using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Flashcards;
public class AppDbContext : DbContext {
    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<Stack> Stacks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Flashcards;Trusted_Connection=True;");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Flashcard>()
            .HasOne(f => f.Stack)
            .WithMany(s => s.Flashcards)
            .HasForeignKey(f => f.StackId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Stack>()
            .HasIndex(s => s.Name)
            .IsUnique();

        modelBuilder.Entity<Stack>()
            .HasData(
            new Stack {
                Id = 1,
                Name = "Questions"
            });

        modelBuilder.Entity<Flashcard>()
            .HasData(
                new Flashcard {
                    Id = 1,
                    StackId = 1,
                    Question = "Test 1",
                    Answer = "Test 1"
                },
                new Flashcard {
                    Id = 2,
                    StackId = 1,
                    Question = "Test 2",
                    Answer = "Test 2"
                }
            );
    }
}

