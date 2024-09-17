using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.Views
{
    public class MainMenu
    {
       public void Menu()
       {
            while (true)
            {
                string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("MAIN MENU")
                    .PageSize(6)
                    .AddChoices(new[]
                    {
                    "1. Manage Stacks", "2. Manage Flashcards", "3. Study", "4. View Study Session", "5. Yearly Report", "6. Exit"
                    }));

                if (int.Parse(choice.Substring(0, 1)) == 6)
                {
                    AnsiConsole.Markup("[red]Exiting.....[/]");
                    Thread.Sleep(1000);
                    return;
                }
                SelectView(int.Parse(choice.Substring(0, 1))); 
            }
       }

        public void SelectView(int choice)
        {
            DatabaseContext _context = new DatabaseContext();

            switch (choice)
            {
                case 1:
                    var stackView = new StackView(_context);
                    stackView.Menu();
                    break;
                case 2:
                    var flashcardView = new FlashcardView(_context);
                    flashcardView.Menu();
                    break;
                case 3:
                    var studySessionService = new StudySessionService(_context);
                    studySessionService.SelectOperation(choice);
                    break;
                case 4:
                    studySessionService = new StudySessionService(_context);
                    studySessionService.SelectOperation(choice);
                    break;
                case 5:
                    studySessionService= new StudySessionService(_context);
                    studySessionService.SelectOperation(choice);
                    break;
                case 6:
                    break;
            }
        }

    }
}
