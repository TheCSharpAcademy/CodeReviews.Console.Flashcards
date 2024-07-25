using Database;

namespace Flashcards
{
    public class App(DbContext dbContext)
    {
        private readonly DbContext db = dbContext;

        public void Run()
        {

        }
    }
}