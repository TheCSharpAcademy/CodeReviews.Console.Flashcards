using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;
using Spectre.Console;

namespace Buutyful.FlashCards.Game;

class GameManager(Deck deck)
{
    private readonly Deck _selectedDeck = deck;
    private readonly SessionRepository _sessionRepository = new();
    private GameState GameState { get; set; } = GameState.Question;
    private bool GameActive { get; set; } = true;
    private int Index { get; set; } = 0;
    private int Score { get; set; } = 0;
    public void Run()
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"[yellow]Studing {_selectedDeck.Name}[/]");
        while (GameActive)
        {
            switch (GameState)
            {
                case GameState.Question: DisplayQeustion(); break;
                case GameState.Answer: DisplayAnswer(); break;
                case GameState.GameOver:
                    {
                        HandleGameOver();
                        DisplayGameOver();
                        break;
                    }
                default: GameActive = false; break;
            }
        }
    }
    private void DisplayQeustion()
    {
        AnsiConsole.MarkupLine($"[yellow]Question[/]: {_selectedDeck.FlashCards[Index].FrontQuestion}");
        GameState = GameState.Answer;
    }
    private void DisplayAnswer()
    {
        var answer = _selectedDeck.FlashCards[Index].BackAnswer;
        var display = answer.Length > 4 ? LongAnserMarked(answer) : ShortAnserMarkd(answer);
        AnsiConsole.MarkupLine($"[yellow]Hint[/]: {display} \n What's ur answer?");

        var inp = Console.ReadLine();
        if ((inp?.ToLowerInvariant()).Equals(answer, StringComparison.CurrentCultureIgnoreCase))
        {
            Score++;
            AnsiConsole.MarkupLine($"[yellow]Correct![/]: {answer}\n Current score {Score}");
        }
        else AnsiConsole.MarkupLine($"[yellow]Nop![/]: correct answer: {answer}\n Current score {Score}");
        Index++;
        if (Index < _selectedDeck.FlashCards.Count) GameState = GameState.Question;
        else GameState = GameState.GameOver;
    }
    private void DisplayGameOver()
    {
        AnsiConsole.MarkupLine($"[yellow]Game over![/]: Final score {Score}");
        AnsiConsole.MarkupLine($"[yellow]Retry?[/]: [yellow][[y]]/[[n]][/]");
        var inp = Console.ReadLine();
        if (inp?.ToLower() == "y")
        {
            GameState = GameState.Question;
            Score = 0;
            Index = 0;
        }
        else GameActive = false;
    }
    private void HandleGameOver()
    {
        var session = new StudySession()
        {
            DeckId = _selectedDeck.Id,
            CreatedAt = DateTime.UtcNow,
            Score = Score
        };
        _sessionRepository.Add(session);
    }
    private string LongAnserMarked(string str) => new(str.Select((c, i) => i % 2 == 0 || c == ' ' ? c : '_').ToArray());
    private string ShortAnserMarkd(string str) => new(str.Select(c => '_').ToArray());
}
public enum GameState
{
    Question,
    Answer,
    GameOver
}