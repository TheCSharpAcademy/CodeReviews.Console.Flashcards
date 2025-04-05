
using Spectre.Console;

class StudyController
{
    StacksDatabaseManager stacksDatabaseManager = new();
    FlashcardsDatabaseManager flashcardsDatabaseManager = new();
    StudySessionDatabaseManager studySessionDatabaseManager = new();
    Stack stack = default;
    public async Task StartAsync()
    {
        stack = await SetStack();
        List<FlashcardDTO> flashcards = await GetFlashCards();
        int? score = 0;

        Console.Clear();
        Shuffle(flashcards);
        foreach(var card in flashcards)
        {
            DisplayData.CardForStudy(card);
            bool? answer = GetInput.StudyAnswer(card.Back);

            // If user inputs exit code
            if (answer == null)
            {
                score = null;
                break;
            }
            if ((bool)answer)
                score += 1;
        }

        if (score != null)
        {
            AnsiConsole.MarkupLine($"[bold green]{score}/{flashcards.Count} answered correctly[/]");
            // insert new log
            await studySessionDatabaseManager.InsertLog(new StudySession(
                stack.Id,
                default,
                DateTime.Now.ToString(),
                (int)score
            ));
            AnsiConsole.MarkupLine("[bold grey]Press Enter to continue[/]");
            Console.Read();
        } // else does not insert (user exited early)
    }

    async Task<Stack> SetStack()
    {
        List<Stack> stackSet = await stacksDatabaseManager.GetLogs();
        return GetInput.Selection(stackSet);
    }

    async Task<List<FlashcardDTO>> GetFlashCards()
    {
        List<Flashcard> flashCardSet = await flashcardsDatabaseManager.GetLogs(stack);
        List<FlashcardDTO> flashcards = [];
        foreach (var card in flashCardSet)
            flashcards.Add(new FlashcardDTO(card));
        
        return flashcards;
    }

    // Shuffle function i found on the internet
    void Shuffle(List<FlashcardDTO> list)
    {
        Random rng = new();
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}