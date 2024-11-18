using FlashCards.Data;
using FlashCards.Models;
using Spectre.Console;

namespace FlashCards.Managers;

internal class StudySessionArea
{

    private List<Card> _cards;
    private int currentStudyStackId;
    private int score;
    // the score is  out of 10.
    // each correct answer grants 2 points
    // a wrong answer will deduct 1 point away.
    public DateTime SessionDateTime { get; set; }
    public int Score
    {
        get => score;
        set => score = value >= 0 && value < 11
           ? value
           : throw new ArgumentOutOfRangeException("Error: error ocurred during setting the value of score, value is invalid.");
    }

    private void LoadCards()
    {
        CardsDBController cardsDBController = new();
        _cards = cardsDBController.ReadAllRows(currentStudyStackId, true);
    }


    public void ShowMenu()
    {
        bool exitMenu = false;
        SessionDateTime = DateTime.Now;
        AnsiConsole.MarkupLine("[yellow]Choose a stack to study: [/]");
        SelectStack();
        do
        {
            Console.Clear();
            try
            {
                ShowCard();
            }
            catch (ArgumentOutOfRangeException)
            {
                AnsiConsole.MarkupLine($"[red]There are no cards in this stack yet!\n[/]Leaving to main menu.");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                exitMenu = true;
                continue;
            }
            

            if (Score == 10)
            {
                Console.WriteLine("Well Done you got a full score!");
                StoreSession();
                break;
            }
            Console.WriteLine("Press any key to continue studying this stack (or press ESC to quit)");
            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                StoreSession();
                break;
            }
        }
        while (!exitMenu);
        
    }
    public void SelectStack()
    {
        StacksManager stacksManager = new();
        currentStudyStackId = stacksManager.ChooseStackMenu();
    }

    private void ShowCard()
    {
        LoadCards();
        try
        {
            var randomCard = GetRandomCard();
            string cardFront = randomCard.front; // this is the question , and will be shown to the user.
            string cardBack = randomCard.back; // this should be the answer and will be compared with the user's input

            var tableAlignment = ConsoleTableExt.TableAligntment.Left;
            TableVisualisationEngine<string>.ViewSingleColumn(cardFront, "Front", tableAlignment); // show the question in a pleasent table view to represent a card view

            string userEntry = GetUserAnswer();
            ProcessEntry(userEntry, cardBack); // compares the strings and calculates the score, shows the correct answer if wrong.
        }
        catch (ArgumentOutOfRangeException)
        {
            throw;
        }
    }

    private Card GetRandomCard()
    {
        CardsDBController cardsDBController = new();
        int cardsNumberInStack = cardsDBController.RowsCount(currentStudyStackId);

        Random random = new();

        int randomCardNumber = random.Next(0, cardsNumberInStack);

        return _cards[randomCardNumber];
    }
    private static string GetUserAnswer()
    {
        return AnsiConsole.Ask<string>("[yellow]Enter the correct answer for this card:\n[/]");
    }
    private void ProcessEntry(string userEntry, string cardBack)
    {
        try
        {
            if (userEntry.Equals(cardBack, StringComparison.CurrentCultureIgnoreCase))
            {
                AnsiConsole.MarkupLine("[green]Your Answer is correct![/]");
                Score += 2;
                return;
            }
            AnsiConsole.MarkupLine($"[red]Your Answer is incorrect![/]\nYour answer = [red]{userEntry}[/], Correct answer = [green]{cardBack}[/]");
            Score--;
        }
        catch (ArgumentOutOfRangeException)
        {
            // only two possible values could throw an error if the value drops below zero and  goes above 10
            if (score < 0)
                score = 0;
            else if (score > 10)
                score = 10;
        }
    }
    private void StoreSession()
    {
        StudyDBController studyDBController = new();
        var studySession = new StudySession() 
        {
            session_date = SessionDateTime,
            score = Score,
            FK_stack_id = currentStudyStackId
        };

        studyDBController.InsertRow(studySession);
        AnsiConsole.MarkupLine("[green]Session Stored Successfully![/]\n(Press Any Key To Continue)");
        Console.ReadKey();
    }


}
