using FlashCards.Enums;
using FlashCards.Services;
using FlashCards.Utilities;
using Spectre.Console;

namespace FlashCards.Views;

public static class ManageStackView
{
    public static void Run(MenuChoice selectedOption)
    {
        bool endMenu = false;
        
        while (!endMenu)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"You selected: [green]{selectedOption.GetDisplayName()}[/]");

            var enumStackViewValues = Enum.GetValues(typeof(StackViewMenu)).Cast<StackViewMenu>().ToList();
            var selectedMenuOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(enumStackViewValues.Select(e => e.GetDisplayName())));
            
            var selectedEnum = enumStackViewValues.FirstOrDefault(e => e.GetDisplayName() == selectedMenuOption);
            Console.Clear();
            AnsiConsole.MarkupLine($"You selected: [green]{selectedEnum.GetDisplayName()}[/]");
            
            switch (selectedEnum)
            {
                case StackViewMenu.ReturnToMenu:
                    endMenu = true;
                    break;
                case StackViewMenu.CreateStack:
                    StackService.InsertStack();
                    break;
                case StackViewMenu.ViewStacks:
                    StackService.ViewAll();
                    Util.AskUserToContinue();
                    break;
                case StackViewMenu.UpdateStack:
                    StackService.UpdateStack();
                    Util.AskUserToContinue();
                    break;
                case StackViewMenu.DeleteStack:
                    StackService.DeleteStack();
                    Util.AskUserToContinue();
                    break;
            }
        }
    }
}