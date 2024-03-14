namespace Flashcards.Controllers;

public class PracticeController
{
    private readonly Repository repository = new();
    private readonly PracticeSessionRepository sessionRepo = new();
    private readonly PracticeView view = new();

    public void StartSession()
    {
        var stacks = repository.GetAllStacks();
        if (stacks.Count == 0) throw new NoStacksFoundException();

        var stack = view.ShowMenu(stacks, "Select a stack to practice:");
        if (stack.Flashcards.Count == 0) throw new NoFlashcardsFoundException();

        StartPractice(stack);
    }

    private void StartPractice(Stack stack)
    {
        var toPracticeFlashcards = new List<Flashcard>(stack.Flashcards);
        var questionsAsked = 0;
        var questionsCorrect = 0;

        var colour = toPracticeFlashcards.Count switch
        {
            <= 10 => "green", // Easy
            <= 30 => "yellow", // Medium
            _ => "red" // Hard
        };

        view.ShowMessage(
            $"This stack has [{colour}]{toPracticeFlashcards.Count}[/] flashcards. Press any key to start.");

        // TODO add a way out of this loop
        do
        {
            var cardToTest = toPracticeFlashcards.First();
            toPracticeFlashcards.Remove(cardToTest);

            view.ShowMessage($"[skyblue1][bold]Front:[/][/] {cardToTest.Question}");
            questionsAsked++;

            view.ShowMessage($"[DarkCyan][bold]Back:[/][/] {cardToTest.Answer}");
            var userAnswerCorrect = view.AskConfirm("Did you answer the question correctly?");

            if (userAnswerCorrect)
            {
                view.ShowMessage(
                    "[grey]Then I will not ask you again this session. Press any key to go to the next question.[/]");
                questionsCorrect++;
            }
            else
            {
                view.ShowMessage(
                    "[grey]Then I will ask you again later this session. Press any key to go to the next question.[/]");
                toPracticeFlashcards.Add(cardToTest);
            }
        } while (toPracticeFlashcards.Count > 0);

        view.ShowMessage(
            $"During your session you were questioned {questionsAsked} times, and answered {questionsCorrect} times correctly.");

        try
        {
            var currentSession = new Session
            {
                StackId = stack.Id,
                Score = (int)Math.Round((double)questionsCorrect / questionsAsked * 100),
                SessionDate = DateTime.UtcNow
            };
            sessionRepo.CreateSession(currentSession);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void ShowPracticeLog()
    {
        var year = view.GetYear(sessionRepo.GetLogYears());

        var pivotData = sessionRepo.GetMonthlyAverageByYear(year);
        view.ShowLog(pivotData, year);
    }
}