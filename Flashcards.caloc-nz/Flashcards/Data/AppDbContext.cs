using Flashcards.Config;
using Flashcards.Helpers;
using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Spectre.Console;

namespace Flashcards.Data;

public class AppDbContext : DbContext
{
    private readonly DatabaseConfig? _dbConfig;

    public AppDbContext(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public AppDbContext()
    {
    }

    public DbSet<Stack> Stacks { get; set; } = null!;
    public DbSet<Flashcard> Flashcards { get; set; } = null!;
    public DbSet<StudySession> StudySessions { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_dbConfig != null)
            optionsBuilder.UseSqlServer(_dbConfig.ConnectionString);
        else
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=FlashcardsDb;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stack>()
            .HasMany(s => s.Flashcards)
            .WithOne(f => f.Stack)
            .HasForeignKey(f => f.StackId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudySession>()
            .HasOne(ss => ss.Stack)
            .WithMany()
            .HasForeignKey(ss => ss.StackId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void EnsureDatabaseAndMigrate()
    {
        try
        {
            var databaseCreator = Database.GetService<IRelationalDatabaseCreator>();

            if (!databaseCreator.Exists())
            {
                Database.Migrate();
                AnsiConsole.MarkupLine("[green]Database created and migrations applied successfully.[/]");
            }
            else
            {
                var pendingMigrations = Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    Database.Migrate();
                    AnsiConsole.MarkupLine("[green]Pending migrations applied successfully.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]Database is already up-to-date.[/]");
                }
            }
        }
        catch (Exception ex) when (ex.Message.Contains("already an object named"))
        {
            AnsiConsole.MarkupLine("[yellow]Warning: Database objects already exist. Skipping creation steps.[/]");
        }
        catch (Exception ex)
        {
            ErrorHelper.DisplayError("Failed to ensure database creation and apply migrations", ex);
            throw;
        }
    }

    public bool TestConnection()
    {
        try
        {
            return Database.CanConnect();
        }
        catch (Exception ex)
        {
            ErrorHelper.DisplayError("Database connection test failed", ex);
            return false;
        }
    }
}