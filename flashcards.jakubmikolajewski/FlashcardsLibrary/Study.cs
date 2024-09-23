using FlashcardsLibrary.Models;
using Spectre.Console;

namespace FlashcardsLibrary;

public class Study : RunCommand<Study>
{
    public void Start(string stackChoice)
    {
        int score = 0;
        List<FlashcardsDTO> list = MapToDTO(Flashcards.FlashcardsList, Stacks.StackList);
        IEnumerable<FlashcardsDTO> filtered = list.Where(f => f.StackName == stackChoice);
        foreach (FlashcardsDTO f in filtered)
        {
            var table = new Table();
            if (f.Front is not null)
                table.AddColumn(f.Front);
            AnsiConsole.Write(table);

            if (f.Back is not null)
                f.Back = f.Back.ToLower().Trim();

            if (GetAnswer().Equals(f.Back))
            {
                AnsiConsole.Markup("[green]Correct!\n[/]");
                score++;
            }
            else
            {
                AnsiConsole.Markup($"[red]Incorrect! Correct answer was: {f.Back}.[/]\n");
            }
        }
        int finalScore = Convert.ToInt32(((double)score / filtered.Count()) * 100);
        AnsiConsole.Markup($"[blue]You scored {finalScore}%.[/]");
        LogStudySession(finalScore, stackChoice);
        Console.ReadLine();
    }

    private string GetAnswer()
    {
        return Validator.ValidateString(Validator.AskForNewName("answer"));
    }

    private void LogStudySession(int score, string stackChoice)
    {
        DatabaseQueries.Run.InsertStudySession(DateTime.Now, score, stackChoice);
    }

    private static List<FlashcardsDTO> MapToDTO(List<Flashcards> flashcards, List<Stacks> stacks)
    {
        return (from flashcard in flashcards
                join stack in stacks on flashcard.StackId equals stack.StackId
                select new FlashcardsDTO
                {
                    Front = flashcard.Front,
                    Back = flashcard.Back,
                    StackName = stack.StackName
                }).ToList();
    }
}

