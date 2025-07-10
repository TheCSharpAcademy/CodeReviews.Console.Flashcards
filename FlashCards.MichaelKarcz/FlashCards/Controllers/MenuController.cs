using Spectre.Console;

namespace FlashCards.Controllers;
internal static class MenuController
{
    internal static void RunMenuLoop()
    {
        int menuChoiceNumber = -1;

        while (menuChoiceNumber != 0)
        {

            string menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("- FLASHCARDS - \nWhat would you like to do?")
                .PageSize(7)
                .AddChoices(new[]
                {
                    "1) Manage decks",
                    "2) Manage flashcards",
                    "3) Start a study session",
                    "4) View past study sessions",
                    "0) [grey]Exit the application[/]"
                }));

            menuChoiceNumber = Int32.Parse(menuChoice.Substring(0, 1));

            AnsiConsole.Clear();

            switch (menuChoiceNumber)
            {
                case 0:
                    AnsiConsole.WriteLine("\nGoodbye!");
                    break;
                case 1:
                    DeckController.MainDeckMenu();
                    break;
                case 2:
                    FlashcardController.MainFlashcardMenu();
                    break;
                case 3:
                    StudySessionController.MainStudySessionMenu();
                    break;
                case 4:
                    StudySessionController.ShowPreviousStudySessions();
                    break;
                default:
                    AnsiConsole.WriteLine("\nAn error has occurred processing your request. The application will now close. Goodbye!");
                    menuChoiceNumber = 0;
                    break;
            }
        }
    }
}
