using Flashcards.Controllers;
using Flashcards.Models;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace Flashcards.Managers;

internal class FlashcardManager
{
    internal FlashcardController flashcardController = new();
    private CardStackController cardStackController = new();
    private (string Name, int Id) StackData;
    private int flashcardId;

    internal void DisplayFlashcardOptions()
    {
        bool loop = true;
        Console.Clear();
        StackData = GetStackChoice();
        if (StackData.Id == 0)
        {
            AnsiConsole.MarkupLine("[red]No stacks found![/]");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            loop = false;
        }
        while (loop)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"Current stack: [blue]{StackData.Name}[/]");
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an option:")
            .AddChoices(["Show flashcards", "Add flashcards", "Edit a flashcard", "Delete a flashcard", "Return"]));

            switch (choice)
            {
                case "Show flashcards":
                    DisplayFlashcards(StackData.Id);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case "Add flashcards":
                    int amount = UserInput.GetIntInput("Enter the amount of flashcards you want to add: ");
                    for (int i = 1; i <= amount; i++)
                    {
                        Console.WriteLine($"Adding flashcard number {i}: ");
                        (string Term, string Definition) flashcard = UserInput.GetFlashcardInput();
                        flashcardController.AddFlashcard(flashcard.Term, flashcard.Definition, StackData.Id);
                    }
                    break;
                case "Edit a flashcard":
                    DisplayFlashcards(StackData.Id);
                    Console.WriteLine("Enter the ID of the flashcard you want to edit or type 0 to return: ");
                    flashcardId = UserInput.GetFlashcardId(flashcardController, StackData.Id);
                    if (flashcardId != 0)
                    {
                        (string Term, string Definition) newFlashcard = UserInput.GetFlashcardInput(edit: true);
                        flashcardController.EditFlashcard(newFlashcard.Term, newFlashcard.Definition, StackData.Id, flashcardId);
                    }
                    break;
                case "Delete a flashcard":
                    DisplayFlashcards(StackData.Id);
                    Console.WriteLine("Enter the ID of the flashcard you want to delete or type 0 to return: ");
                    flashcardId = UserInput.GetFlashcardId(flashcardController, StackData.Id);
                    if (flashcardId != 0)
                    {
                        HandleRemoval(StackData.Id);
                    }
                    break;
                case "Return":
                    loop = false;
                    break;
            }
        }
    }

    internal (string, int) GetStackChoice()
    {
        var stackMap = cardStackController.GetStackNameToIdMap();
        if (stackMap.IsNullOrEmpty())
            return ("", 0);
        var stackChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select the stack:")
            .AddChoices(stackMap.Keys));
        return (stackChoice, stackMap[stackChoice]);
    }

    internal void DisplayFlashcards(int stackId)
    {
        var flashcards = flashcardController.GetFlashcards(stackId);
        if (flashcards.Count() == 0)
        {
            AnsiConsole.MarkupLine("[red]No flashcards found![/]");
        }
        else
        {
            foreach (var flashcard in flashcards)
            {
                AnsiConsole.MarkupLine($"{flashcard.Id}. [Blue]Term[/]: {flashcard.Term}, [Blue]Definition[/]: {flashcard.Definition}");
            }
        }
    }

    internal void HandleRemoval(int stackId)
    {
        int currentCount = flashcardController.GetFlashcardCount(stackId);
        flashcardController.DeleteFlashcard(stackId, flashcardId);
        for (int i = flashcardId + 1; i <= currentCount; i++)
        {
            flashcardController.SetCorrectId(stackId, i);
        }
    }
}