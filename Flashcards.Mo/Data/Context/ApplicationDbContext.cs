using Microsoft.EntityFrameworkCore;
using Flashcards.Domain.Entities;

namespace Flashcards.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Stack> Stacks { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Stack>().ToTable("Stacks");
            modelBuilder.Entity<Flashcard>().ToTable("Flashcards");
            modelBuilder.Entity<StudySession>().ToTable("StudySessions");

           
        }

       
    }
    
}


