using Flashcards.TwilightSaw.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Flashcards.TwilightSaw.Controller;

public class AppDbContext : DbContext
{
    public DbSet<CardStack> CardStacks { get; set; }
    public DbSet<Flashcard> Flashcards { get; set; }

    private readonly IConfiguration _configuration;

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Warning);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CardStack>().HasMany(u => u.Flashcards).WithOne(o => o.CardStack)
            .HasForeignKey(o => o.CardStackId);
    }
}