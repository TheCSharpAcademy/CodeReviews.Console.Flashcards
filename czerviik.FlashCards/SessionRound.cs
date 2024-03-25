namespace FlashCards;

public class SessionRound
{
    public static int RoundNo { get; private set; }
    public static int Score { get; private set; }
    private static int _totalRounds;

    private List<FlashcardSessionDto> _flashcards;
    private Stack _userStack;
    private MenuManager _menuManager;
    private FlashcardSessionDto _currentFlashcard;

    public SessionRound(List<FlashcardSessionDto> flashcardSessionDtos, Stack userStack, MenuManager menuManager, int totalRounds)
    {
        _flashcards = flashcardSessionDtos;
        _userStack = userStack;
        _menuManager = menuManager;
        _totalRounds = totalRounds;
        RoundNo++;
    }
    
    public void Start()
    {
        _currentFlashcard = GetRandomFlashcard(_flashcards);
        DisplayQuestion(_currentFlashcard);
        var userAnswer = GetUserAnswer();
        ValidateAnswer(userAnswer);
    }

    private string GetUserAnswer()
    {
        return UserInput.InputWithSpecialKeys(_menuManager, true);
    }

    private void ValidateAnswer(string userAnswer)
    {
        if (userAnswer.Equals(_currentFlashcard.Answer, StringComparison.CurrentCultureIgnoreCase))
        {
            Score++;
            UserInterface.CorrectAnswer(Score, _totalRounds);
        }
        else
        {
            UserInterface.WrongAnswer(Score, _totalRounds, _currentFlashcard);
        }
    }

    private FlashcardSessionDto GetRandomFlashcard(List<FlashcardSessionDto> flashcards)
    {
        var rnd = new Random();
        int randomIndex = rnd.Next(flashcards.Count - 1);

        var randomFlashcard = flashcards[randomIndex];
        flashcards.RemoveAt(randomIndex);

        return randomFlashcard;
    }

    private void DisplayQuestion(FlashcardSessionDto flashcard)
    {
        UserInterface.StudySessionQuestion(_userStack, flashcard, RoundNo);
    }

    public static void Reset()
    {
        Score = 0;
        RoundNo = 0;
    }

}

