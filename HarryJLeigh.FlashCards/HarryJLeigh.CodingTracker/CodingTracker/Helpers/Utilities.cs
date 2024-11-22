using CodingTracker.Models;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.Helpers;

public static class Utilities
{
    private static readonly SessionService SessionService = new SessionService();
    private static readonly GoalsService GoalsService = new GoalsService();

    public static DateTime GetDatetime(string time, int? id = null)
    {
        DateTime userDateTime;
        string input;
        do
        {
            AnsiConsole.MarkupLine(
                $"[green]Enter [yellow]{time}[/]:  date and time with the format (2024-12-31 13:45)[/]");
            input = Console.ReadLine();


            if (!DateTime.TryParse(input, out userDateTime))
            {
                AnsiConsole.MarkupLine("[red]Error unrecognised input![/]");
            }
        } while (!DateTime.TryParse(input, out userDateTime));

        if (id.HasValue)
        {
            return ConfirmDateTime(time, userDateTime, id.Value);
        }
        return userDateTime;
    }

    private static DateTime ConfirmDateTime(string time, DateTime userDateTime, int id)
    {
        Console.Clear();
        SessionService.ViewSession(id);
        AnsiConsole.MarkupLine($"[Yellow]Currently editing: {time}[/]");
        AnsiConsole.MarkupLine($"[blue]New {time} value: {userDateTime.ToString()}[/]");
        AnsiConsole.MarkupLine(
            "[bold green]Are you sure you want to confirm date/time[/]? Enter [yellow]Yes[/] or [yellow]No[/][bold green] to re-enter the date/time[/]");
        string input = Console.ReadLine();

        if (input == "yes")
        {
            return userDateTime;
        }
        else
        {
            GetDatetime(time, id);
        }

        return userDateTime;
    }

    public static DateTime GetRandomStartDateTime()
    {
        Random random = new Random();
        int year = random.Next(2023, DateTime.Now.Year + 1);
        int month = random.Next(1, 13);

        int day;
        if (month == 2)
        {
            day = random.Next(1, 29);
        } 
        else if (month == 4 || month == 6 || month == 9 || month == 11)
        {
            day = random.Next(1, 31);
        }
        else
        {
            day = random.Next(1, 32);
        }

        int hour = random.Next(0, 24);
        int minutes = random.Next(0, 60);
        return new DateTime(year, month, day, hour, 0 , 0);
    }

    public static DateTime GetRandomEndDateTime(DateTime startDateTime)
    {
        Random random = new Random();
        int hoursToAdd = random.Next(0, 8);
        int minutesToAdd = random.Next(0, 60);
        return startDateTime.AddHours(hoursToAdd).AddMinutes(minutesToAdd);
    }

    public static int GetHours()
    {
        int hours = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the number of [green]hours[/]:")
                .ValidationErrorMessage("[red]That's not a valid number![/]")
                .Validate(hours =>
                    hours > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Please enter a positive number[/]"))
        );
        AnsiConsole.MarkupLine($"You entered: [yellow]{hours}[/]");
        AskUserToContinue();
        return hours;
    }

    public static int GetId(bool sessions = false, bool goals = false)
    {
        int id;
        if (goals) GoalsService.ViewGoals();
        if (sessions) SessionService.ViewAllSessions();

        while (true)
        {
            AnsiConsole.MarkupLine("[green]Enter an ID from the table:[/]");
            string input = Console.ReadLine();

            if (int.TryParse(input, out id))
            {
                if (goals && GoalsService.GetAllGoalIds().Contains(id))
                {
                    return id;
                }
                else if (sessions && SessionService.GetAllSessionIds().Contains(id))
                {
                    return id;
                }
                else
                {
                    AnsiConsole.MarkupLine(
                        "[red]Error: ID not found in the selected table. Please enter a valid ID.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Error: Unrecognized input! Please enter a valid ID from the table.[/]");
            }
        }
    }

    public static void AskUserToContinue()
    {
        AnsiConsole.MarkupLine("[bold green]Press any key to continue...[/]");
        Console.ReadKey();
        AnsiConsole.MarkupLine("");
    }

    public static int AskForInt(string promptMessage)
    {
        while (true)
        {
            string input = AnsiConsole.Prompt(new TextPrompt<string>($"{promptMessage}").PromptStyle("green"));
            if (int.TryParse(input, out int result)) return result;
            AnsiConsole.MarkupLine("[red]Error: Unrecognized input! Please enter a valid integer.[/]");
        }
    }

    public static string AskForOrderInput()
    {
        string input =
            AnsiConsole.Ask<string>(
                "[green]Do you want to order the results in[/] [cyan]ascending (ASC)[/] [green]or[/] [cyan]descending (DESC)[/] [green]order?[/]");
        input = input.Trim().ToUpper();

        switch (input)
        {
            case "ASC":
                return "ASC";
            case "DESC":
                return "DESC";
            default:
                AnsiConsole.MarkupLine("[red]Invalid input! Please enter 'ASC' or 'DESC'.[/]");
                return AskForOrderInput();
        }
    }

    public static bool PromptReturnToMenu()
    {
        string input = AnsiConsole.Ask<string>("[bold green]Type '0' to return to main menu or '1' to continue.[/]");

        return input switch
        {
            "0" => false,
            "1" => true,
            _ => PromptReturnToMenu()
        };
    }

    public static void DisplayUpdateInfo(int id, string newEntry, string previousEntry, string timeType)
    {
        AnsiConsole.MarkupLine($"[cyan]Updated ID: {id}[/]");
        AnsiConsole.MarkupLine(
            $"[cyan]Previous {timeType}:[/] [blue]{previousEntry}[/] [cyan]to New {timeType}:[/] [blue]{newEntry}[/]");
    }
    
    public static bool CheckSessionsExist(List<CodingSession> sessions)
    {
        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine($"[red]No sessions found.[/]");
            return false;
        }

        return true;
    }

    public static bool CheckGoalsExist(List<Goal> goals)
    {
        if (goals.Count == 0)
        {
            AnsiConsole.MarkupLine($"[red]No goals found.[/]");
            return false;
        }

        return true;
    }

    public static string CalculateDuration(DateTime startTime, DateTime endTime)
    {
        TimeSpan duration = endTime - startTime;
        return duration.TotalHours.ToString("F2");
    }
}