using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards;

/// <summary>
/// Represents the database context for the Flashcards application, managing the entities for Flashcards, Stacks, and StudySessions.
/// </summary>
public class AppDbContext : DbContext {
    /// <summary>
    /// Gets or sets the DbSet of Flashcards.
    /// </summary>
    public DbSet<Flashcard> Flashcards { get; set; }

    /// <summary>
    /// Gets or sets the DbSet of Stacks.
    /// </summary>
    public DbSet<Stack> Stacks { get; set; }

    /// <summary>
    /// Gets or sets the DbSet of StudySessions.
    /// </summary>
    public DbSet<StudySession> StudySessions { get; set; }

    /// <summary>
    /// Configures the database connection and provider options for the context.
    /// </summary>
    /// <param name="optionsBuilder">The options builder used to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Flashcards;Trusted_Connection=True;");
    }

    /// <summary>
    /// Configures the model properties and relationships for the entities in the context.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // Configure the relationship between Flashcard and Stack
        modelBuilder.Entity<Flashcard>()
            .HasOne(f => f.Stack)
            .WithMany(s => s.Flashcards)
            .HasForeignKey(f => f.StackId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the relationship between StudySession and Stack
        modelBuilder.Entity<StudySession>()
                .HasOne(s => s.Stack)
                .WithMany()
                .HasForeignKey(s => s.StackId)
                .OnDelete(DeleteBehavior.Cascade);

        // Ensure Stack names are unique
        modelBuilder.Entity<Stack>()
            .HasIndex(s => s.Name)
            .IsUnique();

        // Ensure Flashcard questions are unique
        modelBuilder.Entity<Flashcard>()
            .HasIndex(f => f.Question)
            .IsUnique();

        // Seed initial data for Stack
        modelBuilder.Entity<Stack>()
            .HasData(
            new Stack {
                Id = 1,
                Name = "Questions"
            });

        // Seed initial data for Flashcards
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
