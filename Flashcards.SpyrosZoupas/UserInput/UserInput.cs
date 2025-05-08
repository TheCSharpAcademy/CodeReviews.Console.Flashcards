using Flashcards.DAL;
using Spectre.Console;

namespace Flashcards.UserInput;

public class UserInput
{
    private readonly FlashcardMenu flashcardMenu;
    private readonly StackMenu stackMenu;
    private readonly StudySessionMenu studySessionMenu;

    public UserInput(Controller controller, Validation validation)
    {
        flashcardMenu = new FlashcardMenu(controller, validation);
        stackMenu = new StackMenu(controller, validation);
        studySessionMenu = new StudySessionMenu(controller, validation);
    }

    public void GetMainMenu()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[bold purple on black]Welcome to the Flashcards application![/]");

        while (true)
        {
            AnsiConsole.MarkupLine("[bold purple on black]MAIN MENU[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]Please choose a sub menu:[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]0) Exit Application[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]1) Flashcards[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]2) Stacks[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]3) Study Sessions[/]");
            string input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[italic hotpink3_1 on black]Please type one of the following values only:[/]")
                .AddChoices([
                    "0",
                    "1",
                    "2",
                    "3"
                ]));

            switch (input)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "1":
                    flashcardMenu.GetFlashcardMenu();
                    break;
                case "2":
                    stackMenu.GetStackMenu();
                    break;
                case "3":
                    studySessionMenu.GetStudySessionMenu();
                    break;
            }
        }
    }
}
