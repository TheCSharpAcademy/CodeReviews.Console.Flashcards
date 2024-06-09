using Flashcards.kalsson.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Flashcards.kalsson.Data_Access;

public class FlashcardContext : DbContext
{
    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<Stack> Stacks { get; set; }
    public DbSet<StudySession> StudySessions { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Initialize a new instance of ConfigurationBuilder class.
        var builder = new ConfigurationBuilder()
            // Set the base path for the configuration builder to the current directory.
            .SetBasePath(Directory.GetCurrentDirectory())
            // Add a new configuration source for a JSON configuration file named "appsettings.json".
            .AddJsonFile("appsettings.json");

        // Build the configuration and retrieve the resulting configuration tree.
        var config = builder.Build();

        // Configure the database to use a SQL Server database 
        // The connection string is retrieved from the configuration built earlier under the key "Default".
        options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
    }
}