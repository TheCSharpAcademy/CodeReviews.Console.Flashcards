using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Data;

public class FlashcardsDbContext : DbContext
{
    public FlashcardsDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Stack> Stacks { get; set; }
    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<StudySession> StudySessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Stack
        modelBuilder.Entity<Stack>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Stack>()
            .HasIndex(x => x.Name)
            .IsUnique();

        modelBuilder.Entity<Stack>()
            .HasMany(x => x.Flashcards);

        // Configure the relationship between Stack and StudySession
        modelBuilder.Entity<Stack>()
            .HasMany(stack => stack.StudySessions)
            .WithOne(session => session.Stack)
            .HasForeignKey(session => session.StackId)
            .OnDelete(DeleteBehavior.Cascade);

        // Flashcard
        modelBuilder.Entity<Flashcard>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Flashcard>()
            .HasOne(x => x.Stack)
            .WithMany(x => x.Flashcards)
            .HasForeignKey(x => x.StackId);

        //StudySession
        modelBuilder.Entity<StudySession>()
               .HasKey(ss => ss.Id);

        modelBuilder.Entity<StudySession>()
            .Property(ss => ss.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<StudySession>()
            .Property(ss => ss.Date)
            .IsRequired();

        modelBuilder.Entity<StudySession>()
            .Property(ss => ss.Score)
            .IsRequired();

        // Seed data
        modelBuilder.Entity<Stack>()
            .HasData(
            new Stack
            {
                Id = 1,
                Name = "CSharp"
            },
            new Stack
            {
                Id = 2,
                Name = "SQL"
            });

        modelBuilder.Entity<Flashcard>()
            .HasData(
            new Flashcard
            {
                Id = 1,
                Question = "What is LINQ?",
                Answer = "Language Integrated Query",
                StackId = 1

            },
            new Flashcard
            {
                Id = 2,
                Question = "Define MVC",
                Answer = "Model View Controller",
                StackId = 1

            },
            new Flashcard
            {
                Id = 3,
                Question = "What is OOP?",
                Answer = "Object Oriented Programming",
                StackId = 1

            },
            new Flashcard
            {
                Id = 4,
                Question = "What does SQL stand for?",
                Answer = "Structured Query Language",
                StackId = 2

            },
            new Flashcard
            {
                Id = 5,
                Question = "Define JOIN in SQL",
                Answer = "Combines rows from two or more tables",
                StackId = 2

            },
            new Flashcard
            {
                Id = 6,
                Question = "What is a Primary Key?",
                Answer = "Unique identifier for a record",
                StackId = 2

            });
    }
}