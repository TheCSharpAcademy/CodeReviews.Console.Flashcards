using DataAccess;
using Spectre.Console;
using System.Text;

namespace Flashcards;
public class View
{
    public static void DisplayMainMenu(out int userInput)
    {
        userInput = 0;
        Console.WriteLine();
        Console.WriteLine($"Current Stack: {StackMethod.GetCurrentStack()}");

        Dictionary<string, int> choices = new Dictionary<string, int>
        {
            { "Exit", 0 },
            { "Select Stack", 1 },
            { "Create Stack", 2 },
            { "Delete Stack", 3 },
            { "View Flashhcards", 4 },
            { "Create Flashcard", 5},
            { "Delete Flashcard" , 6 },
            { "Update Flashcard", 7 },
            { "Study", 8 },
            { "Report Card", 9 }
        };

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices(choices.Keys)
            .HighlightStyle(Style.Parse("red"))
            );
        userInput = choices[choice];
    }
    public static async Task Dots(string msg)
    {
        Console.OutputEncoding = Encoding.UTF8;

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots2)
            .SpinnerStyle(Style.Parse("red"))
            .StartAsync(msg, async (StatusContext x) => { await Task.Delay(1000); });   
    }
    public static async Task Check(bool openApp)
    {
        using (MyDbContext db = new MyDbContext())
        {
            db.Database.EnsureCreated();
            await Dots("Checking database connection...");

            if (!db.Database.CanConnect()) AnsiConsole.Write(new Markup("\n[red]No Database Exist[/\n"));
            else
            {
                AnsiConsole.Write(new Markup("\n[red]Database Exist[/]\n"));
                while (openApp)
                {
                    DisplayMainMenu(out int userInput);
                    Logic.Do(userInput, out bool openApp2);
                    openApp = openApp2;
                }
            }
        }
    }
}
