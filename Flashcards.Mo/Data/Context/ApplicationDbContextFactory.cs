using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Flashcards.Data.Context; 
using Flashcards.Data.Repositories;
using Flashcards.Services;
using Flashcards.Domain.Interfaces;
using System.IO;

namespace Flashcards.Data.Context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
        File.AppendAllText("ef_log.txt", "Starting to create DbContext...");
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        File.AppendAllText("ef_log.txt", "Configuration built...");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        File.AppendAllText("ef_log.txt", "Options configured...");
        return new ApplicationDbContext(optionsBuilder.Options);
        }
    }

}
