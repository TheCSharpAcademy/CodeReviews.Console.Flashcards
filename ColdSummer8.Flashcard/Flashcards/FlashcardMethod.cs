using DataAccess;
using Model;
using Spectre.Console;

namespace Flashcards;
internal class FlashcardMethod
{
    internal static void GetAllFlashcards(out bool flag)
    {
        Console.Clear();
        flag = true;
        if (State.selectedStack == null)
        {
            AnsiConsole.Write(new Markup("\n[red]No stack selected. Select a stack[/]\n"));
        }
        else
        {
            using (MyDbContext db = new MyDbContext())
            {
                IEnumerable<Flashcard> flashcard = db.Flashcards
                    .Where(x => x.StackID == State.selectedStackID)
                    .OrderBy(x => x.ID).ToList();
                List<FlashcardDTO> DTOList = State.mapper.Map<List<FlashcardDTO>>(flashcard);

                int counter = 0;
                foreach (Flashcard element in flashcard)
                {
                    counter++;
                    State.idLookup2[counter] = element.ID;
                }

                if (!DTOList.Any())
                {
                    AnsiConsole.Write(new Markup("\n[red]No flashcards. Create a flashcard[/]\n"));
                    flag = false;
                }
                else
                {
                    Table table = new Table();
                    table.Border = TableBorder.Ascii;
                    table.Title = new TableTitle($"[black]{State.selectedStack}'s FLASHCARDS[/]");

                    table.AddColumn("Flashcard ID");
                    table.AddColumn("Question");
                    table.AddColumn("Answer");

                    int displayID = 0;
                    foreach (FlashcardDTO element in DTOList.Where(x => x.StackID == State.selectedStackID))
                    {
                        displayID++;
                        table.AddRow($"{displayID}", $"{element.Question}", $"{element.Answer}");
                    }
                    AnsiConsole.Write(table);
                }
            }
        }
    }
    internal static void CreateFlashcard()
    {
        string? question = string.Empty, answer = string.Empty;
        Console.Clear();

        if (State.selectedStack == null)
        {
            AnsiConsole.Write(new Markup("\n[red]No stack selected. Select a stack[/]\n"));
        }
        else
        {
            GetAllFlashcards(out bool flag);

            Console.Write("Enter Question: ");
            question = Console.ReadLine();

            using (MyDbContext db = new MyDbContext())
            {
                if (db.Flashcards.Any(x => x.Question == question))
                {
                    AnsiConsole.Write(new Markup("[red]Duplicates not allowed. Try Again![/]"));
                    Thread.Sleep(1000);
                    CreateFlashcard();
                }
                else
                {
                    Console.Write("Enter Answer: ");
                    answer = Console.ReadLine();

                    FlashcardDTO flashcardDTO = new FlashcardDTO() { Question = question, Answer = answer, StackID = State.selectedStackID };
                    Flashcard flashcard = State.mapper.Map<Flashcard>(flashcardDTO);

                    db.Add(flashcard);
                    db.SaveChanges();
                    AnsiConsole.Write(new Markup("[red]Added[/]\n"));
                    Console.Clear();
                }
            }
        }
    }
    internal static void DeleteFlashcard()
    {
        Console.Clear();

        if (State.selectedStack == null) AnsiConsole.Write(new Markup("\n[red]No stack selected. Select a stack[/]\n"));
        else
        {
            GetAllFlashcards(out bool flag);

            if (!flag) return;
            else
            {
                Console.Write("Delete Flashcard (Enter ID): ");
                int deleteID = UserInput.NumericInputOnly();

                using (MyDbContext db = new MyDbContext())
                {
                    if (!State.idLookup2.TryGetValue(deleteID, out int actualID))
                    {
                        AnsiConsole.Write(new Markup("\n[red]Flashcard not found[/]\n"));
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                    else
                    {
                        Flashcard? flashcard = db.Flashcards.Find(actualID);
                        db.Flashcards.Remove(flashcard);
                        db.SaveChanges();

                        AnsiConsole.Write(new Markup("\n[red]Deleted[/]\n"));
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
            }
        }
    }
    internal static void UpdateFlashcard()
    {
        string? question = string.Empty, answer = string.Empty;
        Console.Clear();

        if (State.selectedStack == null) AnsiConsole.Write(new Markup("\n[red]No stack selected. Select a stack[/]\n"));
        else
        {
            GetAllFlashcards(out bool flag);

            if (!flag) return;
            else
            {
                Console.Write("Update Flashcard (Enter ID): ");
                int updateID = UserInput.NumericInputOnly();

                using (MyDbContext db = new MyDbContext())
                {
                    if (!State.idLookup2.TryGetValue(updateID, out int actualID))
                    {
                        AnsiConsole.Write(new Markup("\n[red]Flashcard not found[/]\n"));
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                    else
                    {
                        Flashcard? flashcard = db.Flashcards.Find(actualID);

                        Console.Write("\nEnter New Question: ");
                        question = Console.ReadLine();

                        Console.Write("Enter New Answer: ");
                        answer = Console.ReadLine();

                        FlashcardDTO flashcardDTO = new FlashcardDTO() { Question = question, Answer = answer, StackID = State.selectedStackID };
                        Flashcard flashcard2 = State.mapper.Map<Flashcard>(flashcardDTO);

                        flashcard.Question = flashcard2.Question;
                        flashcard.Answer = flashcard2.Answer;
                        db.SaveChanges();

                        AnsiConsole.Write(new Markup("[red]Updated[/]\n"));
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
            }
        }
    }
}
