using System.Globalization;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using STUDY.ConsoleApp.Flashcards.Controllers;
using STUDY.ConsoleApp.Flashcards.Enums;
using STUDY.ConsoleApp.Flashcards.Models.DTOs;

namespace STUDY.ConsoleApp.Flashcards.UI;

public static class ViewStudySessionsHistory
{
    public static void Menu()
    {
        StudySessionsHistoryController studySessionsHistoryController = new();
        StackController stackController = new();
        var sessions = studySessionsHistoryController.GetAllStudySessions();
        if (sessions.IsNullOrEmpty())
        {
            AnsiConsole.MarkupLine("[red]There are no study sessions to view.[/]");
            AnsiConsole.MarkupLine("\nPress Any key to continue...");
            Console.ReadKey();
            return;
        }
        
        while (true)
        {
            AnsiConsole.Clear();
            var menuOptions = AnsiConsole.Prompt(
                new SelectionPrompt<ViewStudySessionsHistoryOptions>()
                    .Title("View Study History")
                    .AddChoices(Enum.GetValues<ViewStudySessionsHistoryOptions>())
            );

            Table table;
            int selectedYear;
            List<ReportDto> tableResult;
            switch (menuOptions)
            {
                case ViewStudySessionsHistoryOptions.ViewStudySessions:
                    table = new();
                    table.AddColumns(new TableColumn("Id").Centered(), new TableColumn("Date").Centered(), new TableColumn("Stack").Centered(), new TableColumn("Score").Centered());
                    
                    foreach (var session in sessions)
                    {
                        var stack = stackController.GetStackById(session.StackId);
                        table.AddRow(session.Id.ToString(), session.SessionDate.ToString(CultureInfo.CurrentCulture), stack.Name,
                            session.Score.ToString());
                    }
                    AnsiConsole.Write(table);
                    break;

                case ViewStudySessionsHistoryOptions.NumberOfSessionsPerMonth:
                    
                    selectedYear = AnsiConsole.Prompt(
                        new SelectionPrompt<int>()
                            .Title("Select Year")
                            .AddChoices(sessions.Select(s => s.SessionDate.Year).Distinct())
                    );

                    tableResult = studySessionsHistoryController.NumberOfSessionsPerMonth(selectedYear);
                    table = new();
                    table.AddColumns(
                        new TableColumn("Stack").Centered(),
                        new TableColumn("January").Centered(),
                        new TableColumn("February").Centered(),
                        new TableColumn("March").Centered(),
                        new TableColumn("April").Centered(),
                        new TableColumn("May").Centered(),
                        new TableColumn("June").Centered(),
                        new TableColumn("July").Centered(),
                        new TableColumn("August").Centered(),
                        new TableColumn("September").Centered(),
                        new TableColumn("October").Centered(),
                        new TableColumn("November").Centered(),
                        new TableColumn("December").Centered()
                    ).Centered().Title($"Number of sessions per month for each stack in {selectedYear}");
                    
                    foreach (var row in tableResult)
                    {
                        table.AddRow(
                            row.StackName.ToString(),
                            row.January.ToString(),
                            row.February.ToString(),
                            row.March.ToString(),
                            row.April.ToString(),
                            row.May.ToString(),
                            row.June.ToString(),
                            row.July.ToString(),
                            row.August.ToString(),
                            row.September.ToString(),
                            row.October.ToString(),
                            row.November.ToString(),
                            row.December.ToString()
                            );
                    }
                    AnsiConsole.Write(table);
                    break;
                
                case ViewStudySessionsHistoryOptions.AverageScorePerMonth:
                    selectedYear = AnsiConsole.Prompt(
                        new SelectionPrompt<int>()
                            .Title("Select Year")
                            .AddChoices(sessions.Select(s => s.SessionDate.Year).Distinct())
                    );
                    tableResult = studySessionsHistoryController.AverageScorePerMonth(selectedYear);
                    table = new();
                    table.AddColumns(
                        new TableColumn("Stack").Centered(),
                        new TableColumn("January").Centered(),
                        new TableColumn("February").Centered(),
                        new TableColumn("March").Centered(),
                        new TableColumn("April").Centered(),
                        new TableColumn("May").Centered(),
                        new TableColumn("June").Centered(),
                        new TableColumn("July").Centered(),
                        new TableColumn("August").Centered(),
                        new TableColumn("September").Centered(),
                        new TableColumn("October").Centered(),
                        new TableColumn("November").Centered(),
                        new TableColumn("December").Centered()
                    ).Title($"Average scores per month for each stack in {selectedYear}");
                    
                    foreach (var row in tableResult)
                    {
                        table.AddRow(row.StackName.ToString(),
                            row.January.ToString(),
                            row.February.ToString(),
                            row.March.ToString(),
                            row.April.ToString(),
                            row.May.ToString(),
                            row.June.ToString(),
                            row.July.ToString(),
                            row.August.ToString(),
                            row.September.ToString(),
                            row.October.ToString(),
                            row.November.ToString(),
                            row.December.ToString()
                        );
                    }
                    AnsiConsole.Write(table);
                    break;
                
                case ViewStudySessionsHistoryOptions.BackToMainMenu:
                    Menus.MainMenu();
                    break;
            }
            AnsiConsole.MarkupLine("\nPress Any key to continue...");
            Console.ReadKey();
        }
    }
}