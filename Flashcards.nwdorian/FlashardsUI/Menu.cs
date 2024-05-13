using FlashcardsLibrary.Models;
using FlashcardsLibrary.Repositories;
using Spectre.Console;
using static FlashardsUI.Enums;

namespace FlashardsUI;
internal class Menu
{
    StacksController stacksController = new(new StacksRepository());
    FlashcardsController flashcardsController = new(new FlashcardsRepository());
    StudySessionsController studySessionsController = new(
        new StudySessionsRepository(),
        new StacksRepository(),
        new FlashcardsRepository());

    internal async Task MainMenu()
    {
        var exit = false;

        while (!exit)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new FigletText("Flashcards")
                    .LeftJustified()
                    .Color(Color.Red));
            
            var selection = UserInput.EnumPrompt<MainMenuOptions>("Select from the menu:");

            switch (selection)
            {
                case MainMenuOptions.Stacks:
                    await StacksMenu();
                    break;
                case MainMenuOptions.Flashcards:
                    Console.Clear();
                    flashcardsController.CurrentStack = await stacksController.Get("Select a stack of flashcards:");
                    if (flashcardsController.CurrentStack.Id == 0)
                    {
                        continue;
                    }
                    await FlashcardsMenu();
                    break;
                case MainMenuOptions.StudySessions:
                    await StudySessionMenu();
                    break;
                case MainMenuOptions.Exit:
                    if (AnsiConsole.Confirm("Are you sure you want to exit?"))
                    {
                        AnsiConsole.WriteLine("\nGoodbye!");
                        exit = true;
                    }
                    else
                    {
                        exit = false;
                    }
                    break;
            }
        }
    }

    internal async Task StacksMenu()
    {
        var exit = false;

        while (!exit)
        {
            Console.Clear();

            var selection = UserInput.EnumPrompt<StacksMenuOptions>("Manage stacks of flashcards\nSelect from the options");

            switch (selection)
            {
                case StacksMenuOptions.ViewAllStacks:
                    await stacksController.GetAll();
                    break;
                case StacksMenuOptions.AddStack:
                    await stacksController.Post();
                    break;
                case StacksMenuOptions.DeleteStack:
                    await stacksController.Delete();
                    break;
                case StacksMenuOptions.UpdateStack:
                    await stacksController.Update();
                    break;
                case StacksMenuOptions.MainMenu:
                    exit = true;
                    break;
            }
        }
    }

    internal async Task FlashcardsMenu()
    {
        var exit = false;

        while (!exit)
        {
            Console.Clear();

            var selection = UserInput.EnumPrompt<FlashcardsMenuOptions>($"Manage flashcards from the [blue]{flashcardsController.CurrentStack.Name}[/] stack\nSelect from the options");

            switch (selection)
            {
                case FlashcardsMenuOptions.ChangeStack:
                    var stack = await stacksController.Get("Select a stack of flashcards:");
                    if (stack.Id == 0)
                    {
                        continue;
                    }
                    flashcardsController.CurrentStack = stack;
                    break;
                case FlashcardsMenuOptions.ViewAllFlashcards:
                    await flashcardsController.GetAll();
                    break;
                case FlashcardsMenuOptions.AddFlashcard:
                    await flashcardsController.Post();
                    break;
                case FlashcardsMenuOptions.DeleteFlashcard:
                    await flashcardsController.Delete();
                    break;
                case FlashcardsMenuOptions.UpdateFlashcard:
                    await flashcardsController.Update();
                    break;
                case FlashcardsMenuOptions.MainMenu:
                    exit = true;
                    break;
            }
        }
    }

    internal async Task StudySessionMenu()
    {
        var exit = false;

        while (!exit)
        {
            Console.Clear();

            var selection = UserInput.EnumPrompt<StudySessionMenuOptions>("Select from the options");

            switch (selection)
            {
                case StudySessionMenuOptions.ViewAllSessions:
                    await studySessionsController.GetAll();
                    break;
                case StudySessionMenuOptions.StartStudySession:
                    await studySessionsController.Post();
                    break;
                case StudySessionMenuOptions.MainMenu:
                    exit = true;
                    break;
            }
        }
    }
}
