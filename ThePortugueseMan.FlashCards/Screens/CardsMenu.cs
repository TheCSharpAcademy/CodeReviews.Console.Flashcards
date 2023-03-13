using AskInputs;
using ObjectsLibrary;
using ConsoleTableExt;
using System;
using System.Reflection;

namespace Screens;

internal class CardsMenu
{
    AskInput askInput = new();
    SettingsLibrary.Settings settings = new();
    DbCommandsLibrary.DbCommands dbCmd = new();
    public void View()
    {
        bool exitMenu = false;
        List<object> optionsString = new List<object> {
            "1 - View Cards",
            "2 - Add a new Card",
            "3 - Update a Card",
            "4 - Delete a Card",
            "0 - Return"};

        while (!exitMenu)
        {
            Console.Clear();
            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Cards")
                .ExportAndWriteLine();
            Console.Write("\n");
            switch (askInput.PositiveNumber("Please select a valid option"))
            {
                case 0: exitMenu = true; continue;
                case 1: ViewCards(); break;
                case 2: AddCard(); break;
                case 3: UpdateCard(); break;
                case 4: DeleteCard(); break;
                default: break;
            }
            askInput.AnyKeyToContinue();
        }
        return;
    }

    private void ViewCards()
    {
        Console.Clear();

        DisplayCardList(dbCmd.Return.AllCards(), "VIEW");
        return;
    }

    private void AddCard()
    {
        Console.Clear();
        int index;
        Card newCard = new();
        Stack stack = new();

        StacksMenu stacks = new();
        stacks.DisplayStackList(dbCmd.Return.AllStacks(),"CHOOSE");

        do
        {
            index = askInput.PositiveNumber("Choose the stack for your card, or 0 to return");
            stack = dbCmd.Return.StackByIndex(index);
        }
        while ((index != 0) && (stack == null));
        if (index == 0) return;

        newCard.StackId = stack.Id;

        newCard.Prompt =
            askInput.AlphasNumbersSpecialUpToLimit(settings.cardPromptCharLimit, "Write the card's prompt.");
        if (newCard.Prompt == "0") return;

        newCard.Answer =
             askInput.AlphasNumbersSpecialUpToLimit(settings.cardAnswerCharLimit, "Write the card's answer.");
        if (newCard.Answer == "0") return;

        if (dbCmd.Insert.IntoTable(newCard)) Console.WriteLine("Card added successfully!");
        else Console.WriteLine("Couldn't add card...");
        return;
    }

    private void UpdateCard()
    {
        Console.Clear();
        Card oldCard = new(), updatedCard = new();
        Stack newStack = new();
        StacksMenu stacks = new();
        int cardIndex, stackIndex;

        DisplayCardList(dbCmd.Return.AllCards(), "UPDATE");

        do
        {
            cardIndex = askInput.PositiveNumber("Write the index of the card you want to update, or 0 to return");
        }
        while ((cardIndex != 0) && !dbCmd.Check.CardByIndex(cardIndex));
        if(cardIndex == 0) return;

        Console.Clear();
        stacks.DisplayStackList(dbCmd.Return.AllStacks(), "CHOOSE");
        do
        {
            stackIndex = askInput.PositiveNumber("Choose the new stack for your card, or 0 to return");
            newStack = dbCmd.Return.StackByIndex(stackIndex);
        }
        while ((stackIndex != 0) && (newStack == null));
        if (stackIndex == 0) return;

        updatedCard.StackId = newStack.Id;

        updatedCard.Prompt =
            askInput.AlphasNumbersSpecialUpToLimit(settings.cardPromptCharLimit, "Write the card's new prompt.");
        if (updatedCard.Prompt == "0") return;

        updatedCard.Answer =
             askInput.AlphasNumbersSpecialUpToLimit(settings.cardAnswerCharLimit, "Write the card's new answer.");
        if (updatedCard.Answer == "0") return;

        if (dbCmd.Update.CardByIndex(cardIndex, updatedCard)) Console.WriteLine("Card updated successfully!");
        else Console.WriteLine("Couldn't update card...");
        return;
    }

    private void DeleteCard()
    {
        Console.Clear();
        int index;

        DisplayCardList(dbCmd.Return.AllCards(), "DELETE");

        do
        {
            index = askInput.PositiveNumber("Write the index of the card you want to delete, or 0 to return");
        }
        while ((index != 0) && (dbCmd.Return.StackByIndex(index) == null));

        if (index == 0) return;

        if (dbCmd.Delete.CardByIndex(index)) Console.WriteLine("Card deleted successfully!");
        else Console.WriteLine("Couldn't delete card...");
        return;
    }

    private void DisplayCardList(List<Card> cardsToDisplay, string title)
    {
        var tableDataDisplay = new List<List<object>>();

        if (cardsToDisplay is not null)
        {
            foreach (Card card in cardsToDisplay)
            {
                tableDataDisplay.Add(
                    new List<object>
                    {
                        card.ViewId,
                        card.Prompt,
                        card.Answer,
                        dbCmd.Return.StackByIndex(card.StackId).Name
                    });
            }
        }
        else
        {
            tableDataDisplay.Add(new List<object> { "", "", "", "" });
            title = "EMPTY";
        }

        ConsoleTableBuilder.From(tableDataDisplay)
            .WithTitle(title)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithColumn("Id", "Prompt", "Answer", "Stack")
            .ExportAndWriteLine();
        return;
    }
}
