using Spectre.Console;
using FlashcardApp.Core.Services.Interfaces;
using FlashcardApp.Console.MessageLoggers;
using FlashcardApp.Core.Models;
using FlashcardApp.Core.DTOs;

namespace FlashcardApp.Console.Menus;

public class MainMenu
{
    private readonly IStackService _stackService;
    private readonly IFlashcardService _flashcardService;
    private readonly IStudySessionService _studySessionService;

    public MainMenu(IStackService stackService, IFlashcardService flashcardService, IStudySessionService studySessionService)
    {
        _stackService = stackService;
        _flashcardService = flashcardService;
        _studySessionService = studySessionService;
    }

    /********************************************  Main menu  ********************************************/
    public async Task DisplayMenu()
    {
        RenderCustomLine("DodgerBlue1", "Flashcard Menu");
        var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select a [blue]function[/]?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(new[] {
                "Add a Stack", "Delete A stack", "Add A Flashcard",
                "Delete A Flashcard", "Study Session", "View Study Sessions By Stack",
                "Average Score Yearly Report", "Exit",
            }));
        await DisplayOtherMenus(choice);
    }

    private async Task DisplayOtherMenus(string choice)
    {
        choice = choice.ToLower().Trim();
        switch (choice)
        {
            case "add a stack":
                await DisplayAddStackMenu();
                break;
            case "delete a stack":
                await DisplayDeleteStackMenu();
                break;
            case "add a flashcard":
                await DisplayAddFlashcardMenu();
                break;
            case "delete a flashcard":
                await DisplayDeleteFlashcardMenu();
                break;
            case "study session":
                await DisplayStudySessionMenu();
                break;
            case "view study sessions by stack":
                await DisplayViewStudySessions();
                break;
            case "average score yearly report":
                await DisplayReportMenu();
                break;
            default:
                Environment.Exit(0);
                break;
        }
    }

    /********************************************   Stack Menu   ********************************************/
    private async Task AddStack(string stackName)
    {
        var result = await _stackService.AddStack(stackName);
        if (!result.IsSuccess)
        {
            MessageLogger.DisplayErrorMessage("The Stack is already exists.");
            return;
        }
        MessageLogger.DisplaySuccessMessage("[green]1[/] stack added.");
        AnsiConsole.Prompt(new TextPrompt<string>("Press any Key to continue").AllowEmpty());
    }

    private async Task<string[]> GetAllStackNames()
    {
        var stacks = await _stackService.GetAllStacks();
        string[] stackNames = stacks.Value.Select(stack => stack.Name).ToArray();
        return stackNames;
    }

    private async Task<Stack> GetStack(string stackName)
    {
        var stack = await _stackService.GetStackByName(stackName);
        return stack.Value;
    }

    private async Task DeleteStack(string stackName)
    {
        var result = await _stackService.DeleteStack(stackName);
        if (!result.IsSuccess)
        {
            MessageLogger.DisplayErrorMessage("The Stack cannot be deleted.");
            return;
        }
        MessageLogger.DisplaySuccessMessage("[green]1[/] stack is deleted.");
        AnsiConsole.Prompt(new TextPrompt<string>("Press any Key to continue").AllowEmpty());
    }

    private async Task DisplayAddStackMenu()
    {
        var stackName = AnsiConsole.Prompt(
        new TextPrompt<string>("Please enter a [blue]stack name[/] or enter 0 to return to [red]main menu: [/]"));
        if (!stackName.Equals("0"))
        {
            await AddStack(stackName);
        }
        await DisplayMenu();
    }

    private async Task DisplayDeleteStackMenu()
    {
        var stackNames = await GetAllStackNames();
        var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select a [red]stack[/] to delete")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(stackNames));
        await DeleteStack(choice);
    }

    /********************************************    Flashcard Menu    ********************************************/
    private async Task DisplayAddFlashcardMenu()
    {
        var response = await CheckPrompt("The flashcard will be for a new stack(Y/N):  ");
        if (response.Equals("y"))
        {
            await DisplayAddStackMenu();
        }
        var stackNames = await GetAllStackNames();
        var flashCardStackName = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
           .Title("Select a [green]stack[/] to add the flashcard to")
           .PageSize(10)
           .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
           .AddChoices(stackNames));
        var question = await CheckPrompt("Please enter a the [green]front[/] of the flashcard or enter 0 to return to [red]main menu: [/]");
        var answer = await CheckPrompt("Please enter a the [green]back[/] of the flashcard or enter 0 to return to [red]main menu: [/]");

        var stack = await GetStack(flashCardStackName);
        await AddFlashcard(new Flashcard { Id = 1, Answer = answer, Question = question, stack = stack });
    }

    private async Task DisplayDeleteFlashcardMenu()
    {
        var stackNames = await GetAllStackNames();
        var flashCardStackName = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
           .Title("Select a [green]stack[/] Where the flashcard resides")
           .PageSize(10)
           .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
           .AddChoices(stackNames));
        var flashcardList = await GetAllFlashcards(flashCardStackName);

        var flashCardQuestionName = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select a [green]stack[/] Where the flashcard resides")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(flashcardList));
        var flashcard = await GetFlashcard(flashCardQuestionName);
        if (flashcard != null)
        {
            await DeleteFlashcard(flashcard);
        }
    }

    private async Task AddFlashcard(Flashcard flashcard)
    {
        var result = await _flashcardService.AddFlashcard(flashcard);
        if (!result.IsSuccess)
        {
            MessageLogger.DisplayErrorMessage("The flashcard is already exists.");
            return;
        }
        MessageLogger.DisplaySuccessMessage("[green]1[/] flashcard added.");
        AnsiConsole.Prompt(new TextPrompt<string>("Press any Key to continue").AllowEmpty());
    }

    private async Task DeleteFlashcard(Flashcard flashcard)
    {
        var result = await _flashcardService.DeleteFlashcardByQuestion(flashcard.Question);
        if (!result.IsSuccess)
        {
            MessageLogger.DisplayErrorMessage(result.ErrorMessage);
            return;
        }
        MessageLogger.DisplaySuccessMessage("[green]1[/] flashcard deleted.");
        AnsiConsole.Prompt(new TextPrompt<string>("Press any Key to continue").AllowEmpty());
    }

    private async Task<string[]> GetAllFlashcards(string name)
    {
        var flashcards = await _flashcardService.GetFlashcardsByStackname(name);
        var flashcardList = flashcards.Value.Select(flashcard => flashcard.Question).OrderBy(q => q).ToArray();
        return flashcardList;
    }

    private async Task<List<FlashcardDTO>> GetFlashcardList(string name)
    {
        var flashcardDTOList = new List<FlashcardDTO>();
        var flashcards = await _flashcardService.GetFlashcardsByStackname(name);
        if (!flashcards.IsSuccess)
        {
            throw new Exception(flashcards.ErrorMessage);
        }
        foreach (var flashcard in flashcards.Value)
        {
            FlashcardDTO flashcardDTO = new FlashcardDTO(flashcard.Question, flashcard.Answer);
            flashcardDTOList.Add(flashcardDTO);
        }
        return flashcardDTOList;
    }

    private async Task<Flashcard> GetFlashcard(string questionName)
    {
        var result = await _flashcardService.GetFlashcardByQuestion(questionName);
        if (!result.IsSuccess)
        {
            MessageLogger.DisplayErrorMessage(result.ErrorMessage);
            return null;
        }
        return result.Value;
    }

    /********************************************   Study Session Menu   ********************************************/
    private async Task DisplayStudySessionMenu()
    {
        try
        {
            int index = 1;
            int score = 0;

            RenderCustomLine("DodgerBlue1", "Study Session Menu");
            var stackNames = await GetAllStackNames();
            var flashCardStackName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
               .Title("Select a [green]stack[/] to study")
               .PageSize(10)
               .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
               .AddChoices(stackNames));
            var flashcardList = await GetFlashcardList(flashCardStackName);
            foreach (FlashcardDTO flashcard in flashcardList)
            {
                var frontTable = CreateSpecterTable("Front");
                DrawTable(frontTable, index, flashcard.Question);
                var answer = await CheckPrompt("Please, enter the answer for the above flashcard: ");
                if (!answer.Equals(flashcard.Answer))
                {
                    MessageLogger.DisplayMessage("Incorrect");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any Key to continue").AllowEmpty());
                }
                else
                {
                    var backTable = CreateSpecterTable("Back");
                    DrawTable(backTable, index, flashcard.Answer);
                    MessageLogger.DisplayMessage("correct");
                    score++;
                    AnsiConsole.Prompt(new TextPrompt<string>($"You current score is [green]{score}[/]. Press any Key to continue").AllowEmpty());
                }
                index++;
            }
            var stack = await GetStack(flashCardStackName);
            var studySession = new StudySession
            {
                Id = 0,
                stack = stack,
                CurrentDate = DateTime.UtcNow,
                Score = score
            };
            MessageLogger.DisplayFinalScoreMessage(score.ToString());
            var result = await _studySessionService.AddStudySession(studySession);
            if (!result.IsSuccess)
            {
                MessageLogger.DisplayErrorMessage(result.ErrorMessage);
            }
            MessageLogger.DisplaySuccessMessage("[green]1[/] study session created.");
            AnsiConsole.Prompt(new TextPrompt<string>("Press any Key to continue").AllowEmpty());
        }
        catch (Exception ex)
        {
            MessageLogger.DisplayErrorMessage(ex.Message);
        }     
    }
    private Table CreateSpecterTable(string head)
    {
        var table = new Table();
        table.AddColumn("[lightskyblue1] FlashcardId [/]");
        table.AddColumn(new TableColumn($"[skyblue1] {head}[/]").Centered());
        table.Border = TableBorder.Rounded;
        table.BorderColor(Color.DodgerBlue1);
        return table;
    }
    private Table CreateRow(Table table, int id, string questionOrAnswer)
    {
        table.AddRow($"[lightskyblue1]{id}[/]", $"[skyblue1]{questionOrAnswer}[/]");
        return table;
    }

    private void DrawTable(Table table, int id, string questionOrAnswer)
    {
        table = CreateRow(table, id, questionOrAnswer);
        AnsiConsole.Write(table);
    }

    /********************************************  View Study Session Menu  ********************************************/
    private async Task DisplayViewStudySessions()
    {
        RenderCustomLine("DodgerBlue1", "Study Session Menu");
        var stackNames = await GetAllStackNames();
        var selectedStackName = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
           .Title("Select a [green]stack[/] to view study sessions")
           .PageSize(10)
           .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
           .AddChoices(stackNames));

        var studySessions = await _studySessionService.GetStudySessionsByStackName(selectedStackName);
        if (!studySessions.IsSuccess)
        {
            MessageLogger.DisplayErrorMessage(studySessions.ErrorMessage);
            return;
        }
        var table = new Table();
        table.AddColumn("[lightskyblue1] Session Date [/]");
        table.AddColumn(new TableColumn($"[skyblue1] score [/]").Centered());
        table.Border = TableBorder.Rounded;
        table.BorderColor(Color.DodgerBlue1);
        foreach (var studySession in studySessions.Value)
        {
            table.AddRow($"[lightskyblue1]{studySession.CurrentDate}[/]", $"[skyblue1]{studySession.Score}[/]");
        }
        AnsiConsole.Write(table);
        int averageScore = (int)studySessions.Value.Average(studySession => studySession.Score);
        MessageLogger.DisplayAverageScoreMessage(averageScore.ToString());
    }

    private async Task DisplayReportMenu()
    {
        var currentYear = await CheckPrompt("Please enter a year in (Format: yyyy):  ");
        var result = await _studySessionService.GetStudySessionsReport(currentYear);
        if (!result.IsSuccess)
        {
            var answer = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
               .Title($"[red]{result.ErrorMessage}[/] Would you like to try again? ")
               .PageSize(5)
               .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
               .AddChoices("Yes", "No"));
            if (!answer.ToLower().Equals("yes"))
            {
                await DisplayMenu();
            }
            await DisplayReportMenu();
        }
        var table = new Table();
        var reports = result.Value;
        var reportDto = new ReportingDTO();
        foreach (var prop in reportDto.GetType().GetProperties())
        {
            table.AddColumn($"[lightskyblue1] {prop.Name} [/]");
        }
        table.Border = TableBorder.Rounded;
        table.BorderColor(Color.DodgerBlue1);
        foreach (var report in reports)
        {
            var row = new List<string>();
            foreach (var prop in report.GetType().GetProperties())
            {
                var value = prop.GetValue(report)?.ToString() ?? string.Empty;
                row.Add(value);
            }
            table.AddRow(row.ToArray());
        };
        AnsiConsole.Write(table.Centered().Title($"[lightskyblue1]{currentYear} [/][blue]Study sessions[/] "));
        AnsiConsole.Prompt(new TextPrompt<string>("Press any Key to continue").AllowEmpty());
        await DisplayMenu();
    }

    /********************************************   Helper functions  ********************************************/
    private static void RenderCustomLine(string color, string title)
    {
        var rule = new Rule($"[{color}]{title}[/]");
        rule.RuleStyle($"{color} dim");
        AnsiConsole.Write(rule);
    }

    public async Task<string> CheckPrompt(string message)
    {
        string prompt = "";
        do
        {
            prompt = AnsiConsole.Prompt(
                new TextPrompt<string>(message).AllowEmpty());
            if (prompt.Equals("0")) await DisplayMenu();
            if (string.IsNullOrEmpty(prompt))
            {
                AnsiConsole.MarkupLine("[red]Empty value is not allowed[/]");

            }
        } while (string.IsNullOrEmpty(prompt));
        return prompt.ToLower();
    }
}