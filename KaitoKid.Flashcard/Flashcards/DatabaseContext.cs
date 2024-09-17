using Flashcards.Models;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

//DatabaseContext class is designed to interact with a SQL Server database using EF Core.

namespace Flashcards
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Stack>? Stack { get; set; }
        public DbSet<Flashcard>? Flashcard { get; set; }
        public DbSet<StudySession>? StudySession { get; set; }
        private string? connectionString = ConfigurationManager.AppSettings.Get("FlashcardsDBConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configuring a one - to - many relationship
            modelBuilder.Entity<Stack>()
                .HasMany(s => s.Flashcards)
                .WithOne(f => f.Stack)
                .HasForeignKey(f => f.StackId);

            modelBuilder.Entity<Stack>()
                .HasMany(s => s.StudySessions)
                .WithOne(ss => ss.Stack)
                .HasForeignKey(ss => ss.StackId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}
