using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Flashcards.glaxxie.Controllers;
using Flashcards.glaxxie.DTO;
using Spectre.Console;

namespace Flashcards.glaxxie.Prompts;

internal class Stack
{
    internal static StackViewer Selection(string title, bool additionOption = false)
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
        return AnsiConsole.Prompt(prompt);
    }

    internal static StackCreation InsertPrompt()
    {
        var name = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Adding new stack]] \n>> Name (leave it blank to cancel):").AllowEmpty());
        // need to make sure this doesnt exist yet
        return new StackCreation(Name: name);
    }

    internal static StackModification? UpdatePrompt()
    {
        StackViewer stack = Selection("Pick a stack to update the name");
        if (stack.StackId == -1) return null;
        string name = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Update stack's name]] \n>> Name (leave blank to cancel)")
                .DefaultValue(stack.Name)
                .AllowEmpty());
        if (string.Equals(stack.Name.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase)) return null;
        return new StackModification(StackId: stack.StackId, Name: name);
    }

    internal static int DeletePrompt()
    {
        StackViewer stack = Selection("Pick a stack to delete");
        return stack.StackId;
    }

    // always remember to list the stack name at top to know the dur dirct
}
