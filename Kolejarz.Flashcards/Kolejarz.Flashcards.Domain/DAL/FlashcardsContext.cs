using Kolejarz.Flashcards.Domain.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kolejarz.Flashcards.Domain.DAL;

public class FlashcardsContext : DbContext
{
    public DbSet<FlashcardsStack> Stacks { get; set; }
    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<StudySession> Sessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TCSA.Flashcards;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flashcard>()
            .ToTable("Cards");

        modelBuilder.Entity<FlashcardsStack>()
            .ToTable("Stacks");

        modelBuilder.Entity<StudySession>()
            .ToTable("Sessions");

        modelBuilder.Entity<FlashcardsStack>()
            .HasKey(s => s.StackId);

        modelBuilder.Entity<FlashcardsStack>()
            .HasIndex(s => s.Name)
            .IsUnique();

        modelBuilder.Entity<FlashcardsStack>()
            .HasMany(s => s.Flashcards);

        modelBuilder.Entity<FlashcardsStack>()
            .HasMany(s => s.Sessions);

        modelBuilder.Entity<Flashcard>()
            .HasKey(s => s.FlashcardId);

        modelBuilder.Entity<Flashcard>()
            .HasOne(f => f.Stack)
            .WithMany(s => s.Flashcards)
            .HasForeignKey(f => f.StackId);

        modelBuilder.Entity<StudySession>()
            .HasKey(s => s.StudySessionId);

        modelBuilder.Entity<StudySession>()
            .HasOne(s => s.Stack)
            .WithMany(s => s.Sessions)
            .HasForeignKey(s => s.StackId);
    }
}
