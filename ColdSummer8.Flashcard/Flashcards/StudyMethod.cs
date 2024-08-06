using DataAccess;
using Model;
using Spectre.Console;

namespace Flashcards;

internal class StudyMethod
{
    internal static void StudySession()
    {
        Console.Clear();
        if (State.selectedStack == null)
        {
            AnsiConsole.Write(new Markup("\n[red]No stack selected. Select a stack[/]\n"));
        }
        else
        {
            using (MyDbContext db = new MyDbContext())
            {
                if (!db.Flashcards.Where(x => x.StackID == State.selectedStackID).Any()) AnsiConsole.Write(new Markup("\n[red]Stack is empty. Create a flashcard[/]\n"));
                else
                {
                    GetAllStudySession();

                    IEnumerable<Flashcard> flashcard = db.Flashcards.Where(x => x.StackID == State.selectedStackID).ToList();
                    List<FlashcardDTO> DTOList = State.mapper.Map<List<FlashcardDTO>>(flashcard);

                    int score = 0;
                    foreach (FlashcardDTO element in DTOList)
                    {
                        Console.Write($"Capital of {element.Question}? ");
                        string? userInput = Console.ReadLine();

                        if (String.Equals(userInput, element.Answer, StringComparison.OrdinalIgnoreCase)) score++;
                    }
                    Console.WriteLine($"Correct Answer: {score} out of {flashcard.Count()}");

                    Study study = new Study() { Attempt = flashcard.Count(), Date = DateTime.Now, Score = score, Name = State.selectedStack, StackID = State.selectedStackID };
                    db.Studies.Add(study);
                    db.SaveChanges();
                }
            }
        }
    }
    internal static void GetAllStudySession()
    {
        Console.Clear();
        using (MyDbContext db = new MyDbContext())
        {
            IEnumerable<Study> study = db.Studies.ToList();
            List<StudyDTO> DTOList = State.mapper.Map<List<StudyDTO>>(study);

            Table table = new Table();
            table.Border = TableBorder.Ascii;
            table.Title = new TableTitle($"[black]Sessions[/]");

            table.AddColumn("Session");
            table.AddColumn("Stack");
            table.AddColumn("Question");
            table.AddColumn("DateTime");
            table.AddColumn("Score");

            foreach (StudyDTO element in DTOList)
            {
                table.AddRow($"{element.ID}", $"{element.Name}", $"{db.Flashcards.Where(x => x.StackID == State.selectedStackID).Count()}", $"{element.Date}", $"{element.Score}");
            }
            AnsiConsole.Write(table);
        }
    }
    internal static void ReportCard()
    {
        Console.Clear();
        using (MyDbContext db = new MyDbContext())
        {
            if (!db.Studies.Any()) AnsiConsole.Write(new Markup("\n[red]No study sessions recorded[/]\n"));
            else
            {
                IEnumerable<IGrouping<int, Study>> group = db.Studies.GroupBy(x => x.StackID).ToList();
                foreach (IGrouping<int, Study> element in group)
                {
                    foreach (Study e in element)
                    {
                        Console.WriteLine($"Attempted tries for {e.Name}: {element.Count()}, " +
                            $"Average score for {e.Name}: {Math.Round(db.Studies.Where(x => String.Equals(x.Name, e.Name)).Average(x => x.Score), 2)}");
                        break;
                    }
                }
            }
        }
    }
}
