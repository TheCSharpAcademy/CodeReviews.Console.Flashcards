
using Flashcards.FunRunRushFlush.Data.Model;
using Flashcards.FunRunRushFlush.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Flashcards.FunRunRushFlush.Services;

public class UserInputValidationService : IUserInputValidationService
{
    private ILogger<UserInputValidationService> _log;

    public UserInputValidationService(ILogger<UserInputValidationService> log)
    {
        _log = log;
    }

    public Stack ValidateUserStackInput(Stack? stack = null)
    {
        AnsiConsole.MarkupLine("[yellow]Please provide the topic for your Flashcard-Stack.[/]");


        long Id = -1;

        var name = AnsiConsole.Prompt(
             new TextPrompt<string>("[yellow]Enter the [green]Topic[/] (max 250 Chars):[/]")
               .Validate(input =>
               {
                   if (string.IsNullOrWhiteSpace(input) || input.Length > 250)
                       return ValidationResult.Error("[red]Please enter a valid Topicname up to 250 characters![/]");
                   return ValidationResult.Success();
               }));

        // If updating existing
        if (stack != null)
        {
            Id = stack.Id;
        }

        var sta = new Stack(Id, name);
        _log.LogInformation("Validated Stack input: {sta}", sta);

        return sta;
    }

    public Flashcard ValidateUserFlashcardInput(Stack stack, Flashcard flashcard = null)
    {
        AnsiConsole.MarkupLine("[yellow]Please provide the Front and the Back of Flashcard.[/]");


        long Id = -1;
        bool solved = false;

        var front = AnsiConsole.Prompt(
             new TextPrompt<string>("[yellow]Enter the [green]Front[/] (max 250 Chars):[/]")
               .Validate(input =>
               {
                   if (string.IsNullOrWhiteSpace(input) || input.Length > 250)
                       return ValidationResult.Error("[red]Please enter a valid Input, up to 250 characters![/]");
                   return ValidationResult.Success();
               }));

        var back = AnsiConsole.Prompt(
             new TextPrompt<string>("[yellow]Enter the [green]Back[/] (max 250 Chars):[/]")
               .Validate(input =>
               {
                   if (string.IsNullOrWhiteSpace(input) || input.Length > 250)
                       return ValidationResult.Error("[red]Please enter a valid Input, up to 250 characters![/]");
                   return ValidationResult.Success();
               }));

        // If updating an existing session
        if (flashcard != null)
        {
            Id = flashcard.Id;
            solved = flashcard.Solved;
        }

        var flash = new Flashcard(Id, stack.Id,front,back, solved);
        _log.LogInformation("Validated Flashcard input: {flash}", flash);

        return flash;
    }
}