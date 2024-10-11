using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.AnaClos.Controllers
{
    public class ConsoleController
    {
        public string MainMenu(List<string> mainOptions)
        {
            Console.Clear();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a [green]function[/]")
                .PageSize(10)
                .AddChoices(mainOptions));

            //return options.IndexOf(selection);
            return selection;
        }

        public string Menu(string title, string color, List<string> mainOptions)
        {
            Console.Clear();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title($"[{color}]{title}[/]")
                .PageSize(10)
                .AddChoices(mainOptions));

            //return options.IndexOf(selection);
            return selection;
        }

        public string GetString(string message)
        {
            return AnsiConsole.Prompt(new TextPrompt<string>($@"[bold blue]{message} [/]"));
        }

        public void ShowMessage(string message, string color)
        {
            AnsiConsole.MarkupLine($"[{color}]{message}[/]\n");
        }
        public void PressKey(string message)
        {
            ShowMessage(message,"blue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
