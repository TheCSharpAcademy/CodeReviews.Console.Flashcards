using DataAccess;
using Model;
using Spectre.Console;
using System.Data;

namespace Flashcards;
internal class StackMethod
{
    internal static string GetCurrentStack()
    {
        using (MyDbContext db = new MyDbContext())
        {
            if (State.selectedStack == null) return "n/a";
            else
            {
                return State.selectedStack;
            }
        }
    }
    internal static void GetAllStacks()
    {
        Console.Clear();
        using (MyDbContext db = new MyDbContext())
        {
            IEnumerable<Stack> stack = db.Stacks.OrderBy(x => x.ID).ToList();
            List<StackDTO> DTOList = State.mapper.Map<List<StackDTO>>(stack);

            int counter = 0;
            foreach (Stack element in stack)
            {
                counter++;
                State.idLookup[counter] = element.ID;
            }

            if (!DTOList.Any())
            {
                AnsiConsole.Write(new Markup("\n[red]No Stack. Create a stack[/]\n"));
                Thread.Sleep(500);
            }
            else
            {
                Table table = new Table();
                table.Border = TableBorder.Ascii;
                table.Title = new TableTitle("[black]STACKS[/]");

                table.AddColumn("Display ID");
                table.AddColumn("Stack Name");

                int displayID = 0;
                foreach (StackDTO element in DTOList)
                {
                    displayID++;
                    table.AddRow($"{displayID}", $"{element.Name}");
                }
                AnsiConsole.Write(table);
            }
        }
    }
    internal static void CreateStack()
    {
        Console.Clear();
        GetAllStacks();

        Console.Write("Enter Stack Name: ");
        string? userInput = Console.ReadLine();

        if (userInput == string.Empty)
        {
            AnsiConsole.Write(new Markup("[red]Invalid Entry. Try Again![/]\n\n"));
            Thread.Sleep(1000);
            CreateStack();
        }
        else
        {
            using (MyDbContext db = new MyDbContext())
            {
                if (db.Stacks.Any(x => x.Name == userInput))
                {
                    AnsiConsole.Write(new Markup("[red]Duplicates not allowed. Try Again![/]\n"));
                    Thread.Sleep(1000);
                    CreateStack();
                }
                else
                {
                    StackDTO stackDTO = new StackDTO() { Name = userInput };
                    Stack stack = State.mapper.Map<Stack>(stackDTO);
                    db.Stacks.Add(stack);
                    db.SaveChanges();

                    AnsiConsole.Write(new Markup("[red]Added[/]\n"));
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
        }
    }
    internal static void SelectStack()
    {
        Console.Clear();
        GetAllStacks();

        using (MyDbContext db = new MyDbContext())
        {
            if (!db.Stacks.Any()) return;
            else
            {
                Console.Write("Select Stack (Enter ID): ");
                int selectID = UserInput.NumericInputOnly();

                if (!State.idLookup.TryGetValue(selectID, out int actualID))
                {
                    AnsiConsole.Write(new Markup("\n[red]Stack not found[/]\n"));
                    Thread.Sleep(1000);
                    Console.Clear();
                }
                else
                {
                    Stack? stack = db.Stacks.SingleOrDefault(x => x.ID == actualID);
                    StackDTO stackDTO = State.mapper.Map<StackDTO>(stack);

                    State.selectedStack = stackDTO?.Name;
                    State.selectedStackID = stackDTO.ID;
                    AnsiConsole.Write(new Markup($"\n[red]{State.selectedStack} selected[/]\n"));
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
        }
    }
    internal static void DeleteStack()
    {
        Console.Clear();
        GetAllStacks();

        using (MyDbContext db = new MyDbContext())
        {
            if (!db.Stacks.Any()) return;
            else
            {
                Console.Write("Delete Stack (Enter ID): ");
                int deleteID = UserInput.NumericInputOnly();

                if (!State.idLookup.TryGetValue(deleteID, out int actualID))
                {
                    AnsiConsole.Write(new Markup("\n[red]Stack not found[/]\n"));
                    Thread.Sleep(1000);
                    Console.Clear();
                }
                else
                {
                    Stack? stack = db.Stacks.Find(actualID);
                    db.Stacks.Remove(stack);
                    db.SaveChanges();

                    if (State.selectedStack != null && State.selectedStack.Equals(stack.Name)) State.selectedStack = "n/a";

                    AnsiConsole.Write(new Markup("\n[red]Deleted[/]\n"));
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
        }
    }
}
