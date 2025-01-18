using Spectre.Console;

using static Flashcards.Program;
using static Flashcards.Manage;
using static Flashcards.Study;
using static Flashcards.Helpers;

namespace Flashcards;

internal static class UserInterface
{
    private static CanvasImage image = new CanvasImage("image.png");
    
    internal static void ShowMenu()
    {
        while (true)
        {
            GetStackNames();
            GetStudySessions();
            
            AnsiConsole.Clear();
            image.MaxWidth(16);
            AnsiConsole.Write(image);
            AnsiConsole.MarkupLine("[bold][yellow2]Flashcards[/][/]");
            
            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What's your choice?")
                    .AddChoices(new[] { "Study", "View study session data", "Manage stacks and flashcards" })
                    .HighlightStyle(Color.DarkOliveGreen1)
                    );
            
            switch (menuChoice)
            {
                case "Study":
                    string stackName = ShowStackMenu("What stack do you want to study from?");
                    StartStudySession(stackName);
                    break;
                case "View study session data":
                    ShowStudySessions();
                    PressAnyKey();
                    break;
                case "Manage stacks and flashcards":
                    var manageChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("What do you want to manage?")
                            .AddChoices(new[] { "Stacks", "Flashcards" })
                            .HighlightStyle(Color.DarkOliveGreen1)
                    );
                    switch (manageChoice)
                    {
                        case "Stacks":
                            var stacksChoice = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title("What do you want to do?")
                                    .AddChoices(new[] { "Show stacks", "Create a new stack", "Delete a stack"} )
                                    .HighlightStyle(Color.DarkOliveGreen1)
                            );
                            switch (stacksChoice)
                            {
                                case "Show stacks":
                                    ShowStacks();
                                    PressAnyKey();
                                    break;
                                case "Create a new stack":
                                    CreateStack();
                                    PressAnyKey();
                                    break;
                                case "Delete a stack":
                                    DeleteStack();
                                    PressAnyKey();
                                    break;
                            }
                            break;
                        case "Flashcards":
                            var flashcardsChoice = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title("What do you want to do?")
                                    .AddChoices(new[] { "Show flashcards", "Create a new flashcard", "Delete a flashcard"} )
                                    .HighlightStyle(Color.DarkOliveGreen1)
                            );
                            switch (flashcardsChoice)
                            {
                                case "Show flashcards":
                                    ShowFlashcards();
                                    PressAnyKey();
                                    break;
                                case "Create a new flashcard":
                                    CreateFlashcard();
                                    PressAnyKey();
                                    break;
                                case "Delete a flashcard":
                                    DeleteFlashcard();
                                    PressAnyKey();
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
    }

    internal static string ShowStackMenu(string title)
    {
        if (stackNames.Any())
        {
            List<string> stackNamesAsString = new List<string>();
            foreach (StackShowDTO stack in stackNames)
            {
                stackNamesAsString.Add(stack.Name);
            }
                    
            var studyChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(title)
                    .AddChoices(stackNamesAsString)
                    .HighlightStyle(Color.DarkOliveGreen1)
            );
            return studyChoice;
        }
        else { PrintError("No stacks found."); PressAnyKey(); UserInterface.ShowMenu(); }
        return null;
    }
}