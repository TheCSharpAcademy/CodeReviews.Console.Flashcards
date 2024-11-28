using FlashCards.Enums;
using FlashCards.Services;
using FlashCards.Utilities;
using Spectre.Console;

namespace FlashCards.Views;
public static class StudyView
{
    internal static void Run(MenuChoice selectedOption)
    {
        bool endMenu = false;
        while (!endMenu)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"You Selected: [green]{selectedOption.GetDisplayName()}[/]");
            var enumStudyViewValues = Enum.GetValues(typeof(StudyViewMenu)).Cast<StudyViewMenu>().ToList();
            var selectedMenuOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What would you like to do?")
                        .AddChoices(enumStudyViewValues.Select(e => e.GetDisplayName())));
            var selectedEnum = enumStudyViewValues.FirstOrDefault(e => e.GetDisplayName() == selectedMenuOption);
            Console.Clear();
            AnsiConsole.MarkupLine($"You selected: [green]{selectedEnum.GetDisplayName()}[/]");

            switch (selectedEnum)
            {
                case StudyViewMenu.ReturnToMenu:
                    endMenu = true;
                    break;
                case StudyViewMenu.ViewAll:
                    StudyService.ViewAll();
                    break;
                case StudyViewMenu.StartStudy:
                    StudyService.Start();
                    break;
            }
        }
    }
}