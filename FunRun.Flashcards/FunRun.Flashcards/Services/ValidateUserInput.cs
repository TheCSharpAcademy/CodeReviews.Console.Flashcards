
using FunRun.Flashcards.Data.Model;
using FunRun.Flashcards.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace FunRun.Flashcards.Services;

public class UserInputValidationService : IUserInputValidationService
{
    private ILogger<UserInputValidationService> _log;

    public UserInputValidationService(ILogger<UserInputValidationService> log)
    {
        _log = log;
    }

    public Stack ValidateUserSessionInput(Stack? stack = null)
    {
        AnsiConsole.MarkupLine("[yellow]Please provide the topic for your Flashcard-Stack.[/]");


        DateTime sessionStart = default;
        DateTime sessionEnd = default;
        long Id = -1;

        var name = AnsiConsole.Prompt(
             new TextPrompt<string>("[yellow]Enter the [green]Topic[/] (max 250 Chars):[/]")
               .Validate(input =>
               {
                   if (string.IsNullOrWhiteSpace(input) || input.Length > 250)
                       return ValidationResult.Error("[red]Please enter a valid Topicname up to 250 characters![/]");
                   return ValidationResult.Success();
               }));



        // If updating an existing session
        if (stack != null)
        {
            Id = stack.Id;
        }

        var sta = new Stack(Id, name);
        _log.LogInformation("Validated Stack input: {sta}", sta);

        return sta;
    }
}