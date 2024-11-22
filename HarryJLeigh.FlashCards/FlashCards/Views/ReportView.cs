using FlashCards.Enums;
using FlashCards.Services;
using FlashCards.Utilities;
using Spectre.Console;

namespace FlashCards.Views;

public static class ReportView
{
    internal static void Run(MenuChoice selectedOption)
    {
        bool endMenu = false;
        
        while (!endMenu)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"You Selected: [green]{selectedOption.GetDisplayName()}[/]");
            string currentStack = StackExtensions.ChooseStack();
            var enumReportViewValues = Enum.GetValues(typeof(ReportViewMenu)).Cast<ReportViewMenu>().ToList();
            var selectedMenuOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(enumReportViewValues.Select(e => e.GetDisplayName())));
            
            var selectedEnum = enumReportViewValues.FirstOrDefault(e => e.GetDisplayName() == selectedMenuOption);
            Console.Clear();
            AnsiConsole.MarkupLine($"You selected: [green]{selectedEnum.GetDisplayName()}[/]");

            switch (selectedEnum)
            {
                case ReportViewMenu.ReturnToMenu:
                    endMenu = true;
                    break;
                case ReportViewMenu.SessionsPerMonth:
                    ReportService.SessionsReport(currentStack);
                    Util.AskUserToContinue();
                    break;
                case ReportViewMenu.AverageScorePerMonth:
                    ReportService.AverageScoreReport(currentStack);
                    Util.AskUserToContinue();
                    break;
            }
        }
    }
}