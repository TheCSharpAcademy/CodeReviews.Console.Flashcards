using Flashcards.TwilightSaw.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Flashcards.TwilightSaw.Controller;

public class AppDbContext : DbContext
{
    private DbSet<CardStack> CardStacks { get; set; }
    private DbSet<Flashcard> Flashcards { get; set; }

    private readonly IConfiguration _configuration;

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CardStack>().HasMany(u => u.Flashcards).WithOne(o => o.CardStack)
            .HasForeignKey(o => o.CardStackId);
    }
}