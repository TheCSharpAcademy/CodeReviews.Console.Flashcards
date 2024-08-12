using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccess;
public class MyDbContext : DbContext
{
    public DbSet<Stack> Stacks { get; set; }
    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<FlashcardDTO> FlashcardsDTO { get; set; }
    public DbSet<StackDTO> StacksDTO { get; set; }
    public DbSet<Study> Studies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server = .; Database = Flashcards; Trusted_Connection = True; TrustServerCertificate = True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Stack
        modelBuilder.Entity<Stack>().HasKey(x => x.ID);
        modelBuilder.Entity<Stack>().HasAlternateKey(x => x.Name);
        modelBuilder.Entity<Stack>().Property(x => x.Name).IsRequired();
        #endregion

        #region Flashcard
        modelBuilder.Entity<Flashcard>().HasKey(x => x.ID);
        modelBuilder.Entity<Flashcard>().HasIndex(x => x.Question).IsUnique();
        modelBuilder.Entity<Flashcard>().Property(x => x.StackID).IsRequired();
        modelBuilder.Entity<Flashcard>().Property(x => x.Question).IsRequired();
        modelBuilder.Entity<Flashcard>().Property(x => x.Answer).IsRequired();
        modelBuilder.Entity<Flashcard>()
            .HasOne(x => x.Stacks)
            .WithMany(x => x.Flashcards)
            .HasForeignKey(x => x.StackID);
        #endregion

        #region Study
        modelBuilder.Entity<Study>().HasKey(x => x.ID);
        modelBuilder.Entity<Study>().Property(x => x.Attempt).IsRequired()
            .HasColumnName("Attempted Questions");
        modelBuilder.Entity<Study>().Property(x => x.Date).IsRequired();
        modelBuilder.Entity<Study>().Property(x => x.Score).IsRequired();
        modelBuilder.Entity<Study>().Property(x => x.Name).IsRequired();
        modelBuilder.Entity<Study>()
            .HasOne(x => x.Stacks)
            .WithMany(x => x.Studies)
            .HasForeignKey(x => x.StackID)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}
