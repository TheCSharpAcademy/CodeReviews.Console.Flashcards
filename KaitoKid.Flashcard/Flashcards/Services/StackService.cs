using Flashcards.Repository;
using Spectre.Console;

namespace Flashcards.Services
{
    public class StackService
    {
        private DatabaseContext _context;
        public StackService(DatabaseContext context)
        {
            this._context = context;
        }

        public void SelectOperation(int choice)
        {
            var repo = new StackRepository(_context);
            var input = new UserInput();
            try
            {


                switch (choice)
                {
                    case 2:
                        Console.Write("Enter Stack Name: ");
                        var stackName = input.GetText();

                        repo.Insert(stackName);
                        break;

                    case 3:
                        repo.GetStack();
                        break;

                    case 4:
                        int rows = repo.GetStack();
                        if (rows == 0) break;


                        Console.Write("\nEnter Stack Name to be Updated: ");
                        stackName = input.GetText();

                        Console.Write("Enter new Stack Name: ");
                        string updatedStackName = input.GetText();

                        repo.Update(stackName, updatedStackName);

                        break;

                    case 5:
                        rows = repo.GetStack();
                        if (rows == 0) break;

                        Console.Write("Enter Stack Name to be Deleted: ");
                        stackName = input.GetText();

                        repo.Delete(stackName);

                        break;
                }
            }
            catch(Exception ex)
            {
                AnsiConsole.Markup($"\n[red]{ex.InnerException.Message}[/]\n\n");
                _context.ChangeTracker.Clear();
            }
            AnsiConsole.Markup("\n[blue]Press enter to continue....[/]");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
