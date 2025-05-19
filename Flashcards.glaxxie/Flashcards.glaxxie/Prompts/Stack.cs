using Flashcards.glaxxie.Controllers;
using Flashcards.glaxxie.DTO;
using Spectre.Console;

namespace Flashcards.glaxxie.Prompts;

internal class Stack
{
    internal static StackViewer? Selection(string title, bool additionOption = false)
    {
        var prompt =  new SelectionPrompt<StackViewer>()
                .Title($"[[{title}]]")
                .PageSize(15)
                .MoreChoicesText("[grey](Move up and down to reveal more option)[/]")
                .AddChoices(StackController.GetAllStacks())
                .WrapAround();
        if (additionOption)
            prompt.AddChoice(new StackViewer(StackId: 0, Name: "Add new stack", Count: 0));
        prompt.AddChoice(new StackViewer(StackId: -1, Name: "Back", Count: -1));
        var stack = AnsiConsole.Prompt(prompt);
        return stack.StackId == -1 ? null : stack;
    }

    internal static StackCreation? InsertPrompt()
    {
        var name = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Adding new stack]] \n>> Name (leave it blank to cancel):")
                .AllowEmpty()
                .Validate(n =>
                {
                    if (string.IsNullOrWhiteSpace(n))
                        return ValidationResult.Success();
                    return StackController.StackExists(n)
                        ? ValidationResult.Error("[red]Stack name already exists![/]")
                        : ValidationResult.Success();
                }));

        if (string.IsNullOrWhiteSpace(name))
            return null;

        return new StackCreation(Name: name.Trim());
    }

    internal static StackModification? UpdatePrompt()
    {
        StackViewer? stack = Selection("Pick a stack to update the name");
        if (stack == null) return null;
        string name = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Update stack's name]] \n>> Name (leave blank to cancel)")
                .DefaultValue(stack.Name)
                .AllowEmpty());
        if (string.Equals(stack.Name.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase)) return null;
        return new StackModification(StackId: stack.StackId, Name: name);
    }

    internal static int DeletePrompt()
    {
        return Selection("Pick a stack to delete")?.StackId ?? -1;
    }
}