using Ohshie.FlashCards.CardsManager;
using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.Game;

public class GameEngine
{
    private DeckDto _deckDto;
    private List<FlashCardDto> _flashCardDtoList = new();

    private Random _random = Random.Shared;

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
    }

    private int Game()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule($"Studying {_deckDto.DeckName}"));
        AnsiConsole.Write(new Rule($"Type \"cancel study\" to stop studying session"));
        
        int guessedRight = 0;
        foreach (var flashCardDto in _flashCardDtoList)
        {
            var answer = AnsiConsole.Ask<string>($"What is this?\n" +
                                                 $"[bold]{flashCardDto.Name}[/]:\n");
           if (answer == "cancel study") return guessedRight;

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

        return guessedRight;
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