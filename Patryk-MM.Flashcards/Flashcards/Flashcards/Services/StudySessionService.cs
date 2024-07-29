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
        var stackNames = await _stackRepository.GetStackNamesAsync();

        // Add the "Cancel" option to the list
        var choices = stackNames.Concat(new[] { "Cancel" });

        // Display the prompt to the user with the combined list of choices
        var stackName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose a stack to study:")
            .AddChoices(choices)
        );

        // Handle the user's choice
        if (stackName == "Cancel") {
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
                AnsiConsole.MarkupLine($"[red]Incorrect. The correct answer is: {flashcard.Answer}[/]");
            }

            AnsiConsole.MarkupLine($"Your score: [aqua]{session.Score}/{session.TotalQuestions}[/]");

        }

        session.DateTime = DateTime.Now;

        // Optionally, save session details to the repository
        await _studySessionRepository.AddAsync(session);

        AnsiConsole.MarkupLine($"Session complete! Your score: [aqua]{session.Score}/{session.TotalQuestions}[/]\n");

    }

    public async Task ViewSessions() {
        var sessions = await _studySessionRepository.GetAllAsync();

        var table = new Table();
        table.AddColumn("Date");
        table.AddColumn("Stack");
        table.AddColumn("Score");

        foreach (var session in sessions) {
            table.AddRow($"{session.DateTime:dd-MM-yyyy HH:mm}", $"{session.Stack.Name}", $"{session.Score}/{session.TotalQuestions}");
        }

        AnsiConsole.Write(table);
    }
}
