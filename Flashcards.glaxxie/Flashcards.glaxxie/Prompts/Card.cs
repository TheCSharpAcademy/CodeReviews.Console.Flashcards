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

    internal static CardCreation? InsertPrompt(StackViewer stack)
    {
        var question = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Front]] Question:").AllowEmpty());
        var response = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Back]] Response:").AllowEmpty());

        if (question.IsNullOrEmpty() || response.IsNullOrEmpty()) return null;
        return new CardCreation(StackId: stack.StackId, Front: question, Back: response);
    }

    internal static CardViewer? UpdatePrompt(StackViewer stack)
    {
        var cardPicked = Selection(CardController.GetCardsFromStack(stack.StackId), $"Pick a card from {stack.Name} to update");
        if (cardPicked == null) return null;

        Console.WriteLine("Leave either part blank to cancel");
        var question = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Front]] Question:").AllowEmpty());
        var response = AnsiConsole.Prompt(
            new TextPrompt<string>("[[Back]] Response:").AllowEmpty());
        if (question.IsNullOrEmpty() || response.IsNullOrEmpty()) return null;

        return new CardViewer(CardId:  cardPicked.CardId, StackId: stack.StackId, Front: question, Back: response);
    }

    internal static IEnumerable<int> DeletePrompt(StackViewer stack)
    {
        var cards = AnsiConsole.Prompt(
            new MultiSelectionPrompt<CardViewer>()
                .Title("Pick card(s) to be deleted")
                .NotRequired()
                .PageSize(20)
                .MoreChoicesText("Move up or down to reveal more cards")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle a card, " +
                    "[green]<enter>[/] to accept)[/]")
                .AddChoices(CardController.GetCardsFromStack(stack.StackId))
            );
        return cards.Select(c => c.CardId);
    }
}