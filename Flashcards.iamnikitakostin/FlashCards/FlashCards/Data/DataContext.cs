using FlashCards.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Data
{
    internal class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<Stack> Stacks { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Phonebook;Trusted_Connection=True;Encrypt=false;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudySession>()
                .HasOne(ss => ss.Stack)
                .WithMany(s => s.StudySessions)
                .HasForeignKey(ss => ss.StackId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Flashcard>()
                .HasOne(ss => ss.Stack)
                .WithMany(s => s.Flashcards)
                .HasForeignKey(ss => ss.StackId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
