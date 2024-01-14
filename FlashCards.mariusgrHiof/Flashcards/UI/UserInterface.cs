using Flashcards.Controllers;
using Spectre.Console;

namespace Flashcards.UI;

public class UserInterface
{
    private readonly StacksController _stacksController;
    private readonly FlashcardsController _flashcardsController;
    private readonly StudySessionsController _studySessionsController;

    public UserInterface(StacksController stacksController, FlashcardsController flashcardsController, StudySessionsController studySessionsController)
    {
        _stacksController = stacksController;
        _flashcardsController = flashcardsController;
        _studySessionsController = studySessionsController;
    }
    public async Task Run()
    {
        AnsiConsole.Write(
        new FigletText("Flashcards")
        .LeftJustified()
        .Color(Color.Blue));

        MainMenu mainMenu = new MainMenu(_stacksController, _flashcardsController, _studySessionsController);

        await mainMenu.ShowMenu();
    }
}