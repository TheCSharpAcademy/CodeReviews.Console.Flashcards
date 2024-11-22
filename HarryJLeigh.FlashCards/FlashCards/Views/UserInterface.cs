using FlashCards.Enums;
using FlashCards.Utilities;
using Spectre.Console;

namespace FlashCards.Views;

public class UserInterface
{
    internal void Run()
    {
        bool endApp = false;
        
        while (!endApp)
        {
            Console.Clear();
            var enumMenuValues = Enum.GetValues(typeof(MenuChoice)).Cast<MenuChoice>().ToList();
            var selectedMenuOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(enumMenuValues.Select(e => e.GetDisplayName())));
            
            var selectedEnum = enumMenuValues.FirstOrDefault(e => e.GetDisplayName() == selectedMenuOption);
            AnsiConsole.MarkupLine($"You selected: [green]{selectedEnum.GetDisplayName()}[/]");

            switch (selectedEnum)
            {
                case MenuChoice.exit:
                    endApp = true;
                    break;
                case MenuChoice.ManageStacks:
                    ManageStackView.Run(selectedEnum);
                    break;
                case MenuChoice.ManageFlashCards:
                    ManageFlashcardView.Run(StackExtensions.ChooseStack());
                    break;
                case MenuChoice.Study:
                    StudyView.Run(selectedEnum);
                    break;
                case MenuChoice.ViewStudySessionData:
                    ReportView.Run(selectedEnum);
                    break;
            }
        }
    }
}