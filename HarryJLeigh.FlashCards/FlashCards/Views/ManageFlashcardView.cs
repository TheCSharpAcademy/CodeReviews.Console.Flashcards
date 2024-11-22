using FlashCards.Enums;
using FlashCards.Services;
using FlashCards.Utilities;
using Spectre.Console;

namespace FlashCards.Views;

public static class ManageFlashcardView
{
    public static void Run( string currentStack)
    {
        bool endMenu = false;
        string stackInUse = currentStack;
        
        while (!endMenu)
        {
            Console.Clear();
            var enumStackMenuValues = Enum.GetValues(typeof(FlashcardViewMenu)).Cast<FlashcardViewMenu>();
      
                var menuChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Current Working Stack: [yellow]{stackInUse}[/]")
                        .AddChoices(enumStackMenuValues.Select(e => e.GetDisplayName())));
                var selectedEnum = enumStackMenuValues.FirstOrDefault(e => e.GetDisplayName() == menuChoice);
                AnsiConsole.MarkupLine($"You selected: [green]{selectedEnum.GetDisplayName()}[/]");
            
            // create enums and Change cases to enum display names
            switch (selectedEnum)
            {
                case FlashcardViewMenu.ReturnToMenu:
                    endMenu = true;
                    break;
                case FlashcardViewMenu.ChangeStack:
                    stackInUse = FlashcardService.ChangeCurrentStack();
                    break;
                case FlashcardViewMenu.ViewAll:
                    FlashcardService.ViewAllFlashcards(stackInUse);
                    Util.AskUserToContinue();
                    break;
                case FlashcardViewMenu.ViewAmountOfFlashCards:
                    FlashcardService.ViewSpecificAmount(stackInUse);
                    Util.AskUserToContinue();
                    break;
                case FlashcardViewMenu.CreateFlashcard:
                    FlashcardService.CreateFlashcard(stackInUse);
                    break;
                case FlashcardViewMenu.EditFlashcard:
                    FlashcardService.EditFlashcard(stackInUse);
                    break;
                case FlashcardViewMenu.DeleteFlashcard:
                    FlashcardService.DeleteFlashcard(stackInUse);
                    break;
            }
        }
    }
}