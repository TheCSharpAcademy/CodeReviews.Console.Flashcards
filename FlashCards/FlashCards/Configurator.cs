using System.Configuration;

namespace FlashCards
{
    internal class Configurator
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["FlashCardDatabase"].ConnectionString;
    }
}
