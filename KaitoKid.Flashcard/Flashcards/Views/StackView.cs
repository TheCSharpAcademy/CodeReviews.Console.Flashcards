using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.Views
{
    public class StackView
    {
        private DatabaseContext _context;

        public StackView(DatabaseContext context)
        {
            this._context = context;
        }

        public void Menu()
        {
            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("MANAGE STACK MENU")
                    .PageSize(7)
                    .AddChoices(new[]
                    {
                    "1. Return to Main Menu", "2. Add Stack", "3. View Stack", "4. Edit Stack", "5. Delete Stack"
                    }));
                var service = new StackService(_context);
                int opt = int.Parse(choice.Substring(0, 1));

                if (opt == 1) break;

                service.SelectOperation(opt);
            }
        }
    }
}



