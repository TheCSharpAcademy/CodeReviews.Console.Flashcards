using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Flashcards.Models {
    public class AppDbContext : DbContext{
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<Stack> Stacks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Flashcards;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flashcard>()
                .HasOne(f => f.Stack)
                .WithMany(s => s.Flashcards)
                .HasForeignKey(f => f.StackId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
