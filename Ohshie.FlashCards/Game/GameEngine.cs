using Ohshie.FlashCards.CardsManager;
using Ohshie.FlashCards.StacksManager;
using Ohshie.FlashCards.StudySessionManager;

namespace Ohshie.FlashCards.Game;

public class GameEngine
{
    private readonly DeckDto _deckDto;
    private List<FlashCardDto> _flashCardDtoList = new();
    private readonly Random _random = Random.Shared;
    private StudySessionService _sessionService = new();
    
    public GameEngine(DeckDto deckDto)
    {
        _deckDto = deckDto;
    }

    public void Initialize()
    {
        FlashcardService flashcardService = new();
        _flashCardDtoList = flashcardService.FlashCardDtoList(_deckDto);
        
        ShuffleCards();

        var correctAnswers = Game();
        
        _sessionService.CreateSession(correctAnswers,_deckDto.DeckName!);
        
        PostGameStat(correctAnswers);
    }

    private int Game()
    {
        AnsiConsole.Clear();

        int guessedRight = 0;
        if (GameLoop(ref guessedRight)) return guessedRight;

        return guessedRight;
    }

    private void PostGameStat(int correctAnswers)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule($"Whew! You studied {_deckDto.DeckName} and did quite well!"));
        
        AnsiConsole.Markup($"You answered [red]{correctAnswers}[/] out of {_deckDto.AmountOfFlashcards} correctly!\n" +
                           $"Press any key to go back to menu");
        Console.ReadKey();
    }

    private bool GameLoop(ref int guessedRight)
    {
        foreach (var flashCardDto in _flashCardDtoList)
        {
            AnsiConsole.Write(new Rule($"Studying {_deckDto.DeckName}"));
            AnsiConsole.Write(new Rule($"Type \"cancel study\" to stop studying session"));
            
            var answer = AnsiConsole.Ask<string>($"What is this?\n" +
                                                 $"[bold]{flashCardDto.Name}[/]:\n");

            if (answer == "cancel study") return true;

            if (answer == flashCardDto.Content)
            {
                guessedRight++;
                AnsiConsole.WriteLine("Correct!\n" +
                                      "Press any key to proceed");
                Console.ReadKey();
                AnsiConsole.Clear();
            }
            else
            {
                AnsiConsole.WriteLine($"Wrong! Correct answer {flashCardDto.Content}\n" +
                                      $"Press any key to proceed");
                Console.ReadKey();
                AnsiConsole.Clear();
            }
        }

        return false;
    }

    private void ShuffleCards()
    {
        int n = _flashCardDtoList.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            var flashCardDto = _flashCardDtoList[k];
            _flashCardDtoList[k] = _flashCardDtoList[n];
            _flashCardDtoList[n] = flashCardDto;
        }
    }
}