using Flashcard_Application.DataServices;
using Flashcards.Models;
using Flashcards.Validation;
using Spectre.Console;

namespace Flashcards.UI;

public class CreateStack
{
    public static void CreateStackPrompt()
    {
        StackDatabaseServices stackdatabaseServices = new StackDatabaseServices();

        string stackName = AnsiConsole.Prompt<string>(new TextPrompt<string>("[blueviolet]Please enter a name for the Stack: [/]"));
        if (!ValidationService.UniqueStackNameCheck(stackName))
        {
            AnsiConsole.Markup("\n\n Sorry a stack with that name already exists.\n\n");
            CreateStackPrompt();
        }
        else
        {
            string stackDescription = AnsiConsole.Prompt<string>(new TextPrompt<string>("[blueviolet]Please enter a description for the Stack: [/]"));

            StackDTO stack = new();
            stack.StackName = stackName;
            stack.StackDescription = stackDescription;

            stackdatabaseServices.InsertStack(stack);

            Thread.Sleep(1000); //adding 1sec pause for UX
            AnsiConsole.Markup("[chartreuse2]\n\n\t\tStack creation successful[/]");
            Thread.Sleep(2000); //adding 2sec pause for UX
            Console.Clear();
            MainMenu.MainMenuPrompt();
        }
    }
}
