
using Spectre.Console;

class StudyController
{
    
    public async Task Start()
    {
        Stack stack = await SetStack();
        List<FlashcardDTO> flashcards = await GetFlashCards(stack);
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
            await DataBaseManager<StudySession>.InsertLog(
                stack.Id,
                DateTime.Now.ToString(),
                score
            );
            AnsiConsole.MarkupLine("[bold grey]Press Enter to continue[/]");
            Console.Read();
        } // else does not insert (user exited early)
    }

    async Task<Stack> SetStack()
    {
        List<Stack> stackSet = await DataBaseManager<Stack>.GetLogs();
        return GetInput.Selection(stackSet);
    }

    async Task<List<FlashcardDTO>> GetFlashCards(Stack currentStack)
    {
        string query = "Stacks_Id = " + currentStack.Id;
        List<Flashcard> flashCardSet = await DataBaseManager<Flashcard>.GetLogs(query);
        List<FlashcardDTO> flashcards = [];
        foreach (var card in flashCardSet)
            flashcards.Add(new FlashcardDTO(card));
        
        return flashcards;
    }

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