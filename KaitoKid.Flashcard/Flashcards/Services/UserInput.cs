using Spectre.Console;

namespace Flashcards.Services
{
    public class UserInput
    {
        public string GetText()
        {
            string text;
            while (true)
            {
                text = Console.ReadLine();
                if (text == "")
                {

                    AnsiConsole.Markup("[red]Empty word not allowed[/]");
                    Console.Write("\nEnter again: ");
                }
                else
                    break;
            }
            return text;
        }

        public int GetInt()
        {
            int id;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out id))
                {
                    return id;
                }
                Console.Write("Enter integer: ");
            }
        }
    }
}
