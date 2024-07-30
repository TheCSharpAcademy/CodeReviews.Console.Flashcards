using Flashcards.Models;
using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Services;
public class StudySessionService {
    private readonly IStudySessionRepository _studySessionRepository;
    private readonly IStackRepository _stackRepository;

    public StudySessionService(IStudySessionRepository studySessionRepository, IStackRepository stackRepository) {
        _studySessionRepository = studySessionRepository;
        _stackRepository = stackRepository;
    }

    public async Task StartSession() {
        var session = new StudySession();

        // Fetch the list of stack names from the repository
        var stackNames = await _stackRepository.GetStackNamesAsync(s => s.Flashcards.Count >= 1);

        // Add the "Cancel" option to the list
        var choices = stackNames.Concat(new[] { "[red]Cancel[/]" });

        // Display the prompt to the user with the combined list of choices
        var stackName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose a stack to study:")
            .AddChoices(choices)
        );

        // Handle the user's choice
        if (stackName == "[red]Cancel[/]") {
            AnsiConsole.WriteLine("Operation cancelled.");
            return; // Exit the method or perform any other cancellation logic
        }

        session.Stack = await _stackRepository.GetStackByNameAsync(stackName);

        AnsiConsole.MarkupLine($"Managing stack: [aqua]{session.Stack.Name}[/]");
        AnsiConsole.MarkupLine($"Cards in stack: [aqua]{session.Stack.Flashcards.Count}[/]\n");

        session.TotalQuestions = AnsiConsole.Prompt(
            new TextPrompt<int>("How many questions do you want to answer?")
                .PromptStyle("green")
                .ValidationErrorMessage("[red]That's not a valid number![/]")
                .Validate(number => {
                    if (number <= 0) {
                        return ValidationResult.Error("[red]Value must be at least 1[/]");
                    } else if (number > session.Stack.Flashcards.Count) {
                        return ValidationResult.Error($"[red]Value cannot be higher than the amount of flashcards in the stack ({session.Stack.Flashcards.Count})[/]");
                    } else {
                        return ValidationResult.Success();
                    }
                })
        );

        // Ensure we do not select more flashcards than available
        var selectedFlashcards = session.Stack.Flashcards.OrderBy(x => Guid.NewGuid()).Take(session.TotalQuestions).ToList();

        foreach (var flashcard in selectedFlashcards) {

            // Present the flashcard question
            AnsiConsole.MarkupLine($"\nQuestion: [aqua]{flashcard.Question}[/]");

            // Optionally, prompt user for the answer
            var userAnswer = AnsiConsole.Ask<string>("Your answer:");

            // Check the user's answer (optional)
            if (userAnswer.Equals(flashcard.Answer, StringComparison.OrdinalIgnoreCase)) {
                AnsiConsole.MarkupLine("[green]Correct![/]");
                session.Score++; // Increase the score for a correct answer
            } else {
                AnsiConsole.MarkupLine($"[red]Incorrect. The correct answer is: [aqua]{flashcard.Answer}[/][/]");
            }

            AnsiConsole.MarkupLine($"Your score: [aqua]{session.Score}/{session.TotalQuestions}[/]");

        }
        session.DateTime = DateTime.Now;

        // Optionally, save session details to the repository
        await _studySessionRepository.AddAsync(session);

        AnsiConsole.MarkupLine($"\nSession complete! Your score: [aqua]{session.ScorePercentage:P}[/]\n");

    }

    public async Task ViewSessions() {
        var sessions = await _studySessionRepository.GetAllAsync(session => session.Stack);

        var table = new Table();
        table.AddColumn("Date");
        table.AddColumn("Stack");
        table.AddColumn("Score");
        table.AddColumn("Score%");

        foreach (var session in sessions) {
            table.AddRow($"{session.DateTime:dd-MM-yyyy HH:mm}", $"{session.Stack.Name}", $"{session.Score}/{session.TotalQuestions}", $"{session.ScorePercentage:P}");
        }

        AnsiConsole.Write(table);
    }

    public async Task GenerateReports() {
        // Prompt the user for the year with validation
        int year = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the year for the report (e.g., 2024):")
                .Validate(input => {
                    // Check if the year is within a valid range
                    if (input < 1900 || input > DateTime.Now.Year) {
                        return ValidationResult.Error("Please enter a year between 1900 and the current year.");
                    }
                    return ValidationResult.Success();
                })
        );

        // Fetch the monthly averages and sum of sessions from the repository
        var monthlyAverages = await _studySessionRepository.GetMonthlyAveragesAsync(year);
        var sumOfSessions = await _studySessionRepository.GetSumOfSessionsAsync(year);

        // Create and configure the table for average scores
        var avgTable = new Table()
            .Title("Average score per Month per Stack");
        avgTable.AddColumn("Stack Name");
        avgTable.AddColumn("Jan");
        avgTable.AddColumn("Feb");
        avgTable.AddColumn("Mar");
        avgTable.AddColumn("Apr");
        avgTable.AddColumn("May");
        avgTable.AddColumn("Jun");
        avgTable.AddColumn("Jul");
        avgTable.AddColumn("Aug");
        avgTable.AddColumn("Sep");
        avgTable.AddColumn("Oct");
        avgTable.AddColumn("Nov");
        avgTable.AddColumn("Dec");

        // Add rows to the average scores table
        foreach (var avg in monthlyAverages) {
            avgTable.AddRow(
                avg.StackName,
                avg.Jan.ToString("F2"),
                avg.Feb.ToString("F2"),
                avg.Mar.ToString("F2"),
                avg.Apr.ToString("F2"),
                avg.May.ToString("F2"),
                avg.Jun.ToString("F2"),
                avg.Jul.ToString("F2"),
                avg.Aug.ToString("F2"),
                avg.Sep.ToString("F2"),
                avg.Oct.ToString("F2"),
                avg.Nov.ToString("F2"),
                avg.Dec.ToString("F2")
            );
        }

        // Write the average scores table to the console
        AnsiConsole.Write(avgTable);

        // Create and configure the table for sum of sessions
        var sumTable = new Table()
            .Title("Sum of Sessions per Month per Stack");
        
        sumTable.AddColumn("Stack Name");
        sumTable.AddColumn("Jan");
        sumTable.AddColumn("Feb");
        sumTable.AddColumn("Mar");
        sumTable.AddColumn("Apr");
        sumTable.AddColumn("May");
        sumTable.AddColumn("Jun");
        sumTable.AddColumn("Jul");
        sumTable.AddColumn("Aug");
        sumTable.AddColumn("Sep");
        sumTable.AddColumn("Oct");
        sumTable.AddColumn("Nov");
        sumTable.AddColumn("Dec");

        // Add rows to the sum of sessions table
        foreach (var sum in sumOfSessions) {
            sumTable.AddRow(
                sum.StackName,
                sum.Jan.ToString(),
                sum.Feb.ToString(),
                sum.Mar.ToString(),
                sum.Apr.ToString(),
                sum.May.ToString(),
                sum.Jun.ToString(),
                sum.Jul.ToString(),
                sum.Aug.ToString(),
                sum.Sep.ToString(),
                sum.Oct.ToString(),
                sum.Nov.ToString(),
                sum.Dec.ToString()
            );
        }

        // Write the sum of sessions table to the console
        AnsiConsole.Write(sumTable);
    }


}
