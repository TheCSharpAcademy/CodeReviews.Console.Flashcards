using Spectre.Console;
using Flashcards.DTO;
using Flashcards.Enums;

namespace Flashcards.Services;

public class InputHandler
{
    public void PauseForContinueInput()
    {
        AnsiConsole.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public bool ConfirmAction(string actionPromptMessage)
    {
        if (!AnsiConsole.Confirm(actionPromptMessage))
        {
            Utilities.DisplayCancellationMessage("Operation cancelled.");
            PauseForContinueInput();
            return false;
        }

        return true;
    }

    public TEnum PromptMenuSelection<TEnum>() where TEnum : struct, Enum
    {
        string selectedOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title(ConfigSettings.menuTitle)
            .PageSize(ConfigSettings.pageSize)
            .AddChoices(
                Enum.GetNames(typeof(TEnum))
                    .Select(Utilities.SplitCamelCase)));

        return Enum.Parse<TEnum>(selectedOption.Replace(" ", ""));
    }

    public StackDto PromptForSelectionListStacks(IEnumerable<StackDto> stacks, string promptMessage)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<StackDto>()
                .Title(promptMessage)
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to see more log entries)[/]")
                .UseConverter(stack => stack.StackName!)
                .AddChoices(stacks));
    }

    public string PromptForNonEmptyString(string promptMessage)
    {
        string userInput = AnsiConsole.Prompt(
            new TextPrompt<string>(promptMessage)
                .PromptStyle("yellow")
                .Validate(input =>
                {
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        var errorMessage = "[red]Input cannot be empty.[/]";
                        return ValidationResult.Error(errorMessage);
                    }
                }));

        return userInput.Trim();
    }

    public FlashCardDto PromptListSelectionFlashCard(IEnumerable<FlashCardDto> flashcards, string promptMessage)
    {
        FlashCardDto cancelOption = new FlashCardDto { CardID = 0, Front = "", Back = "" };
        var updatedSelectionSet = flashcards.Append(cancelOption); 

        return AnsiConsole.Prompt(
            new SelectionPrompt<FlashCardDto>()
                .Title(promptMessage)
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to see more log entries)[/]")
                .UseConverter(entry =>
                    entry.CardID == 0 ? "[red](Cancel this operation)[/]\n" :
                    $"[bold yellow]ID:[/] {entry.CardID}\n" +
                    $"    [bold cyan]Front:[/] {entry.Front}\n" +
                    $"    [bold magenta]Back:[/] {entry.Back}\n")
                .AddChoices(updatedSelectionSet));
    }

    public IEnumerable<EditablePropertyFlashCard> PromptForEditFlashCardPropertiesSelection()
    {
        return AnsiConsole.Prompt(
            new MultiSelectionPrompt<EditablePropertyFlashCard>()
                .Title("Select properties you want to edit:")
                .NotRequired()
                .PageSize(10)
                .InstructionsText("[grey](Press [blue]<space>[/] to toggle a property, [green]<enter>[/] to accept, [yellow]<enter>[/] with no selections will cancel update)[/]")
                .AddChoices(Enum.GetValues<EditablePropertyFlashCard>()));
    }

    public int PromptForPositiveInteger(string promptMessage)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>(promptMessage)
                .Validate(input =>
                {
                    if (!int.TryParse(input.ToString().Trim(), out int parsedQuantity))
                    {
                        return ValidationResult.Error("[red]Please enter a valid integer number.[/]");
                    }

                    if (parsedQuantity <= 0)
                    {
                        return ValidationResult.Error("[red]Please enter a positive number.[/]");
                    }

                    return ValidationResult.Success();
                }));
    }
}
