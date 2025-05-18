using Dapper;
using Flashcards.Data;
using Flashcards.Repository;
using Flashcards.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flashcards
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton<IFlashcardRepository, FlashcardRepository>();
            services.AddSingleton<IStackRepository,StackRepository>();
            services.AddSingleton<IStudySessionRepository, StudySessionRepository>();

            services.AddSingleton<UI>();

            var serviceProvider = services.BuildServiceProvider();

            DbInitializer.CreateDefaultTables();

            //DbInitializer.Initialize(configuration.GetConnectionString("defaultConnection"));

            var mainmenu = serviceProvider.GetRequiredService<UI>();

            mainmenu.Start();
            
        }
    }
}
