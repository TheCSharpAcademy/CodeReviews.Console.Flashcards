using AdityaFlashCards.Database.DatabaseManager;
using Microsoft.Extensions.Configuration;

namespace AdityaFlashCards
{
    internal class Application
    {
        public DatabaseManager Db { get; set; }
        public Application() {
            IConfiguration configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Db = new DatabaseManager(configurationBuilder);
        }
             
    }
}
