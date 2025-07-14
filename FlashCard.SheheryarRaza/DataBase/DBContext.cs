using Microsoft.EntityFrameworkCore;
using FlashCard.SheheryarRaza.Entities;

namespace FlashCard.SheheryarRaza.DataBase
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<FlashCards> FlashCards { get; set; }
        public DbSet<Stacks> Stacks { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stacks>(entity =>
            {
                entity.HasIndex(s => s.Name).IsUnique();
                entity.Property(s => s.Name).IsRequired();
                entity.Property(s => s.Description).IsRequired();
            });

            modelBuilder.Entity<FlashCards>(entity =>
            {
                entity.HasOne(fc => fc.Stack)
                      .WithMany(s => s.FlashCards)
                      .HasForeignKey(fc => fc.StackId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(fc => fc.Question).IsRequired();
                entity.Property(fc => fc.Answer).IsRequired();
            });


            modelBuilder.Entity<StudySession>(entity =>
            {
                entity.HasOne(ss => ss.Stack)
                      .WithMany()
                      .HasForeignKey(ss => ss.StackId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(ss => ss.StartTime).IsRequired();
                entity.Property(ss => ss.EndTime).IsRequired();
                entity.Property(ss => ss.Score).IsRequired();
                entity.Property(ss => ss.TotalQuestions).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
