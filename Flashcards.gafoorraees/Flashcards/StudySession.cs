using Flashcards.Models;
using Flashcards.Tables;
using Spectre.Console;

namespace Flashcards;

public class StudySession
{
    public static void StartStudySession()
    {
        Console.Clear();

        StacksUI.DisplayStacks();
        
        Console.WriteLine("\nPlease enter the name of the stack that you would like to study:\n");
        
        var stackName = Console.ReadLine().Trim();
        var stackID = Stacks.ReturnStackID(stackName);

        if (stackID == 0)
        {
            Console.WriteLine("Stack not found. Please try again.\n");
            StartStudySession();
            return;
        }

        List<Flashcard> flashcards = FlashcardsTable.GetFlashcardsFromStack(stackID);
        List<FlashcardDTO> flashcardDTOS = flashcards.Select(f => MapToDTO(f)).ToList();

        var studySession = new StudySessionDTO
        {
            StackID = stackID,
            Date = DateTime.Now,
            Score = 0
        };

        if (flashcardDTOS.Count == 0)
        {
            Console.WriteLine("No flashcards available in this stack. Please add flashcards first.\n");
            return;
        }

        for (int i = 0; i < flashcardDTOS.Count; i++)
        {
            var flashcard = flashcardDTOS[i];

            Console.WriteLine($"\nQuestion: {flashcard.Question}\n");

            string userAnswer = "";

            while (string.IsNullOrWhiteSpace(userAnswer))
            {
                Console.WriteLine("Your answer:\n");
                userAnswer = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(userAnswer))
                {
                    Console.WriteLine("Answer cannot be blank. Please try again.\n");
                }
            }

            if (string.Equals(userAnswer, flashcard.Answer, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\nCorrect!\n");
                studySession.Score++;
            }
            else
            {
                Console.WriteLine($"\nIncorrect. The correct answer is: {flashcard.Answer}\n");
            }

            if (i < flashcardDTOS.Count - 1)
            { 
                Console.WriteLine("Press Enter to continue to the next question.\n");
                Console.ReadLine();
            }
        }

        Console.WriteLine($"You have completed all cards for this stack! Your score: {studySession.Score}/{flashcardDTOS.Count}");
        Console.WriteLine("Enter any key to return to the main menu.\n");
        Console.ReadLine();

        StudySessions.SaveStudySession(studySession);
    }

    public static FlashcardDTO MapToDTO(Flashcard flashcard)
    {
        return new FlashcardDTO
        {
            Question = flashcard.Question,
            Answer = flashcard.Answer
        };
    }

    public static void DisplayStudySessions()
    {
        Console.Clear();

        var studySessions = StudySessions.GetStudySessions();

        AnsiConsole.Write(new Markup("[bold underline]Study Sessions[/]\n").Centered());

        if (!studySessions.Any())
        {
            Console.WriteLine("No study sessions found, please study more!");
            Console.WriteLine("Press any key to return to the main menu\n");
            Console.ReadLine();
            Utility.ReturnToMenu();
        }

        var table = new Table();

        table.AddColumn(new TableColumn("[bold]Stack[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Date[/]")).Centered();
        table.AddColumn(new TableColumn("[bold]Score[/]")).Centered();

        table.Border(TableBorder.Ascii);
        table.ShowHeaders = true;
        table.BorderColor(Color.Grey);
        table.ShowFooters = false;

        foreach (var session in studySessions)
        {
            table.AddRow(
                session.StackName,
                session.Date.ToString("yyyy-MM-dd HH:mm"),
                session.Score.ToString()
            );
        }

        AnsiConsole.Write(table);

        Console.WriteLine("\nPress any key to return to the main menu\n");
        Console.ReadLine();
    }
}
