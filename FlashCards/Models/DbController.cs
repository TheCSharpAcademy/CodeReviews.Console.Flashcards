using System.Configuration;

namespace FlashCards.Models
{
    public abstract class DbController
    {
        protected string? connectionString;

        protected DbController()
        {
            connectionString= ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        }


    }
}
