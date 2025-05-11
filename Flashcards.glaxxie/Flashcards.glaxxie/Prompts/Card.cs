using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flashcards.glaxxie.Controllers;
using Flashcards.glaxxie.DTO;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace Flashcards.glaxxie.Prompts;

internal class Card
{
    internal static CardViewer Selection(List<CardViewer> cards, string title, bool additionOption = false)
    {
        var prompt = new SelectionPrompt<CardViewer>()
                .Title($"[[{title}]]")
                .PageSize(15)
                .MoreChoicesText("[grey](Move up and down to reveal more option)[/]")
                .AddChoices(cards)
                .WrapAround();
        if (additionOption)
            prompt.AddChoice(new CardViewer(StackId: 0));
        prompt.AddChoice(new CardViewer());
        return AnsiConsole.Prompt(prompt);
    }

    internal static CardCreation? InsertPrompt()
    {
        // adding card action should be in a loop. which mean move the stack picking outside in menu
        // maybe change the stack /card selection process to return null
        var stack = Stack.Selection("Pick a pack");
        if (stack.StackId == -1) return null;
        Console.WriteLine("Leave either part blank to cancel");
        var question = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Front side]] \n>> question:").AllowEmpty());

        var response = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Back side]] \n>> response:").AllowEmpty());

        if (question.IsNullOrEmpty() || response.IsNullOrEmpty()) return null;
        return new CardCreation(StackId: stack.StackId, Front: question, Back: response);
    }

    internal static CardViewer? UpdatePrompt()
    {
        var stack = Stack.Selection("Choose a stack");
        if (stack.StackId == -1 || stack.Count == 0) return null;

        var cardPicked = Selection(CardController.GetCardsFromStack(stack.StackId), $"Pick a card from {stack.Name} to update");
        if (cardPicked.StackId == -1) return null;

        Console.WriteLine("Leave either part blank to cancel");
        var question = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Front side]] \n>> question:").AllowEmpty());
        var response = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Back side]] \n>> response:").AllowEmpty());
        if (question.IsNullOrEmpty() || response.IsNullOrEmpty()) return null;

        return new CardViewer(CardId:  cardPicked.CardId, StackId: stack.StackId, Front: question, Back: response);
    }

    internal static IEnumerable<int> DeletePrompt()
    {
        var stack = Stack.Selection("Pick a pack");
        if (stack.StackId == -1) return [];
        //Console.WriteLine("Pick the card(s) to be deleted or choose nothing to cancel");
        // multi selection, but how to handle going back
        var cards = AnsiConsole.Prompt(
            new MultiSelectionPrompt<CardViewer>()
                .Title("Pick card(s) to be deleted")
                .NotRequired()
                .PageSize(20)
                .MoreChoicesText("Move up or down to reveal more cards")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle a fruit, " +
                    "[green]<enter>[/] to accept)[/]")
                .AddChoices(CardController.GetCardsFromStack(stack.StackId))
            );
        return cards.Select(c => c.CardId);
    }
}
