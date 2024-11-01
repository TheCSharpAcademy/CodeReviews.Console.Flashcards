using Flashcards.TwilightSaw.Domain;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.TwilightSaw.Controller;

public class AppDbContext : DbContext
{
    private DbSet<CardStack> CardStacks { get; set; }
    private DbSet<Flashcard> Flashcards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("localdb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CardStack>().HasMany(u => u.Flashcards).WithOne(o => o.CardStack)
            .HasForeignKey(o => o.StackId);
    }
}