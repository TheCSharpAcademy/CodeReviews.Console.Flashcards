using FlashCards.Models;
using FlashCards.Dtos;
using FlashCards.Data;
using FlashCards.Mappers;
using Spectre.Console;

namespace FlashCards.Managers;

internal class CardsManager : ModelManager<Card>
{
    public enum CardOperation
    {
        ShowCardsInStack,
        AddCardToStack,
        SeeAmountOfCardsInStack,
        DeleteCard,
    }
    private int currentStackID;

    private List<CardDto> cardsDtos;
    private List<Card> cards;
    readonly CardsDBController cardsDBController = new();
    public CardsManager()
    {
        SelectStack();
        LoadCardsWithStackId(currentStackID);
    }
    private void SelectStack()
    {
        List<StackDto> stacksDtos;
        List<Stack> stacks;
        StacksDBController stacksDBController = new();
        StacksManager stacksManager = new StacksManager();

        stacks = stacksDBController.ReadAllRows();

        stacksDtos = stacks.Select(
                s => s.ToStackDto())
                .ToList();

        currentStackID = stacksManager.ChooseStackMenu();

    }
    private void LoadCardsWithStackId(int currentStackID)
    {
        cards = cardsDBController.ReadAllRows(currentStackID);

        cardsDtos = cards.Select(
                c => c.ToCardDto())
                .ToList();
    }
    public void ShowMenu()
    {
        if (currentStackID == -1)
            return;
        var userOption = AnsiConsole.Prompt(
            new SelectionPrompt<CardOperation>()
            .Title("[yellow]Choose an operation: [/]")
            .AddChoices(Enum.GetValues<CardOperation>()));

        switch (userOption)
        {
            case CardOperation.ShowCardsInStack:
                ShowCards();
                Console.ReadKey();
                break;
            case CardOperation.AddCardToStack:
                AddNewModel();
                Console.ReadKey();
                break;
            case CardOperation.SeeAmountOfCardsInStack:
                var numOfCardsInStack = cardsDBController.RowsCount(currentStackID);
                AnsiConsole.MarkupLine($"[yellow]This stack has {numOfCardsInStack} of cards in it[/]\n(Press Any Key To Continue)");
                Console.ReadKey();
                break;
            case CardOperation.DeleteCard:
                var cardNumber = ChooseCard();
                if (cardNumber == -1)
                    break;
                DeleteModel(cardNumber);
                Console.ReadKey();
                break;
        }
    }

    private void ShowCards()
    {
        var cardsWithSequence = cardsDBController.ReadAllRows(currentStackID, true);

        var cardsWithSequenceDtos = cardsWithSequence.Select(
                c => c.ToCardDto())
                .ToList();

        List<string> columnNames = ["Card Number", "Front", "Back"];
        TableVisualisationEngine<CardDto>.ViewAsTable(cardsWithSequenceDtos, ConsoleTableExt.TableAligntment.Left, columnNames);
    }

    private int ChooseCard()
    {
        bool exitMenu = false;
        do
        {
            Console.Clear();
            ShowCards();
            AnsiConsole.MarkupLine("[yellow]Enter a card number (Or enter 'quit' to exit)[/]");
            string? readResult = Console.ReadLine();

            if (string.IsNullOrEmpty(readResult))
            {
                AnsiConsole.MarkupLine("[red]Error- Invalid input[/]");
                continue;
            }
            string userEntry = readResult.Trim();
            exitMenu = userEntry.Equals("quit", StringComparison.CurrentCultureIgnoreCase);
            if (exitMenu)
                continue;

            if (!int.TryParse(userEntry, out int userChoice) || userChoice < 1 || userChoice > cardsDtos.Count)
            {
                AnsiConsole.MarkupLine("[red]Error- Invalid input, please choose a valid card number.[/]");
                continue;
            }

            var chosenCard = cards[userChoice - 1]; // User entry is 1-based; list index is 0-based
            AnsiConsole.MarkupLine($"[green]You selected card: {chosenCard.cardnumber}![/]");
            return chosenCard.cardnumber;
        }
        while (!exitMenu);
        return -1;
    }




    protected override void AddNewModel()
    {
        var frontText = AnsiConsole.Ask<string>("[yellow]Enter what you want to be on front of the card\n[/]");
        var backText = AnsiConsole.Ask<string>("[yellow]Enter what you want to be on back of the card\n[/]");
        var card = new Card { FK_stack_id = currentStackID, front = frontText, back = backText };
        cardsDBController.InsertRow(card);
        AnsiConsole.MarkupLine("[green]Card Added Succesfully![/]");
    }

    protected override void DeleteModel(int cardNumber)
    {
        AnsiConsole.MarkupLine("[red]Are you sure you want to delete this card?\n[white](To Confirm Deletion Press Enter)[/][/]");
        if (Console.ReadKey().Key == ConsoleKey.Enter)
        {
            cardsDBController.DeleteRow(cardNumber);
            AnsiConsole.MarkupLine("[green]Card Deleted Succesfully![/]");
            return;
        }
        AnsiConsole.MarkupLine("[red]Card Deletion Cancelled! (Press Any Key To Continue)[/]");
    }

    protected override void UpdateModel(int stackId, Card modifiedCard)
    {
        throw new NotImplementedException();
    }
}
