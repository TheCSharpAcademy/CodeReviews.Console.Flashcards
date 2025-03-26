using Flashcards.Study.Models;
using Flashcards.Study.Models.Domain;
using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Study
{
    public class UserInputs
    {
        public static int mainmenuselection()
        {
            AnsiConsole.Markup("please choose you choice between[yellow] 0 to 4[/]:\n");
            bool res1 = int.TryParse(Console.ReadLine(), out int userChoiceMenu);
            while (!res1 || userChoiceMenu < 0 || userChoiceMenu > 4)
            {
                AnsiConsole.Markup("[red] WrongChoice[/]\n please choose correct number between [green]0 to 4[/]\n");
                res1 = int.TryParse(Console.ReadLine(), out userChoiceMenu);
            }
            return userChoiceMenu;
        }
        public static int ManageStacksMenu()
        {
            AnsiConsole.Markup("press [yellow]'0'[/] [blue] to exit back to main menu[/]\npress [yellow]'1'[/] to [green]insert a stack[/] or '2'[red] to delete a stack[/]\n");
            AnsiConsole.Markup("press [yellow]3[/] to enter into a current stack\n");
            int userChoice;
            bool result = int.TryParse(Console.ReadLine(), out userChoice);
            while (!result || userChoice < 0 || userChoice > 3)
            {
                AnsiConsole.Markup("Wrong choice enter again\n");
                result = int.TryParse(Console.ReadLine(), out userChoice);
            }
            return userChoice;
        }
        public static int ManageFlashcardsMenu()
        {
            AnsiConsole.Markup("press [yellow]'0'[/] [blue] to exit back to main menu[/]\n'1'[red] to delete a flashcard[/]\n");
            int userChoice;
            bool result = int.TryParse(Console.ReadLine(), out userChoice);
            while (!result || userChoice < 0 || userChoice > 1)
            {
                AnsiConsole.Markup("Wrong choice enter again\n");
                result = int.TryParse(Console.ReadLine(), out userChoice);
            }
            return userChoice;
        }
        public static int ManageSingleStack(string currentstack)
        {
            AnsiConsole.MarkupLine($"The Current Working stack:[greenyellow]{currentstack}[/] ");
            AnsiConsole.MarkupLine($"press [yellow] 0 [/] To return to previous Menu ");
            AnsiConsole.MarkupLine($"press [yellow] 1 [/] To change the [greenyellow] current stack[/] ");
            AnsiConsole.MarkupLine($"press [yellow] 2 [/] To View flashcards of [greenyellow] current Stack[/]");
            AnsiConsole.MarkupLine($"press [yellow] 3 [/] To [green]Create [/]a new [dodgerblue1] flashcard [/] of [greenyellow] current Stack[/]");
            AnsiConsole.MarkupLine($"press [yellow] 4 [/] To [red]Delete [/] a [dodgerblue1] flashcard [/] of [greenyellow] current Stack[/]");
            AnsiConsole.MarkupLine($"press [yellow] 5 [/] To [darkmagenta] Edit[/] a [dodgerblue1] flashcard [/] of [greenyellow] current Stack[/]");
            bool res = int.TryParse(Console.ReadLine(), out int userChoice);
            while (!res || userChoice < 0 || userChoice > 5)
            {
                AnsiConsole.MarkupLine("[red]Wrong choice [/]please pick [yellow]an integer between 0 to 5[/] ");
                res = int.TryParse(Console.ReadLine(), out userChoice);
            }
            return userChoice;
        }
        public static int deleteFlashcardMenu(int total)
        {
            AnsiConsole.MarkupLine($"Enter the ID from 1 to {total}");
            bool res = int.TryParse(Console.ReadLine(), out int userChoice);
            while (!res || userChoice < 1 || userChoice > total)
            {
                AnsiConsole.Markup($"Wrong choice enter again ID should be an integer from 1 to {total}\n");
                res = int.TryParse(Console.ReadLine(), out userChoice);
            }
            return userChoice;

        }
        public static string PickSingleStack(List<string> stacks)
        {
            var stackpicked = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                 .Title("[olive]Please pick the stack you want to Explore!![/]?")
                                 .PageSize(10)
                                 .MoreChoicesText("[grey](Please pick the stack you want to Explore!)[/]")
                                 .AddChoices(stacks));
            return stackpicked;
        }
        public static FlashcardDTO CreateNewFlashcard()
        {
            var flashcard = new FlashcardDTO();
            AnsiConsole.MarkupLine("Please enter the[green] front[/] of the flashcard");
            string front = Console.ReadLine();
            AnsiConsole.MarkupLine("Please enter the[green] back[/] of the flashcard");
            string back = Console.ReadLine();
            flashcard.Front = front;
            flashcard.Back = back;
            return flashcard;
        }
        public static int DeleteFlashcardforStack(List<FlashcardDTO> flashcardsDTO)
        {
            Table flashcardsTable = new Table();
            flashcardsTable.Title = new TableTitle("Flashcards Management Area");
            flashcardsTable.Border = TableBorder.HeavyEdge;
            var properties = typeof(FlashcardDTO).GetProperties();
            foreach (var prop in properties)
            {
                flashcardsTable.AddColumn(prop.Name);
            }
            flashcardsDTO.ForEach(x => flashcardsTable.AddRow(Markup.Escape(x.ID.ToString()), Markup.Escape(x.Front.ToString()), Markup.Escape(x.Back.ToString())));
            AnsiConsole.Write(flashcardsTable);
            int userChoice;
            AnsiConsole.MarkupLine("Please enter the [yellow]ID [/]of the flashcard to Delete:");
            bool res = int.TryParse(Console.ReadLine(), out userChoice);
            while (!res || userChoice < 1 || userChoice > flashcardsDTO.Count)
            {
                AnsiConsole.MarkupLine("[red]Wrong choice[/] please enter the correct ID of the flashcard to delete");
                res = int.TryParse(Console.ReadLine(), out userChoice);
            }
            return userChoice;
        }
        public static int EditFlashcardforStack(List<FlashcardDTO> flashcardsDTO)
        {
            Table flashcardsTable = new Table();
            flashcardsTable.Title = new TableTitle("Flashcards Management Area");
            flashcardsTable.Border = TableBorder.HeavyEdge;
            var properties = typeof(FlashcardDTO).GetProperties();
            foreach (var prop in properties)
            {
                flashcardsTable.AddColumn(prop.Name);
            }
            flashcardsDTO.ForEach(x => flashcardsTable.AddRow(Markup.Escape(x.ID.ToString()), Markup.Escape(x.Front.ToString()), Markup.Escape(x.Back.ToString())));
            AnsiConsole.Write(flashcardsTable);
            int userChoice;
            AnsiConsole.MarkupLine("Please enter the [yellow]ID [/]of the flashcard to Edit:");
            bool res = int.TryParse(Console.ReadLine(), out userChoice);
            while (!res || userChoice < 1 || userChoice > flashcardsDTO.Count)
            {
                AnsiConsole.MarkupLine("[red]Wrong choice[/] please enter the correct ID of the flashcard to Edit");
                res = int.TryParse(Console.ReadLine(), out userChoice);
            }
            return userChoice;
        }
        public static FlashcardDTO GeteditedFlashcardDTO()
        {
            AnsiConsole.MarkupLine("Please enter the[pink3] front[/] of the flashcard");
            string front = Console.ReadLine();
            AnsiConsole.MarkupLine("Please enter the[pink3] back[/] of the flashcard");
            string back = Console.ReadLine();
            var flashcard = new FlashcardDTO()
            {
                Front = front,
                Back = back
            };
            return flashcard;
        }
        public static double StudyAStack(string stackpicked)
        {
            int totalcount = 0;
            int correctcount = 0;
            double result;
            Dictionary<string, string> frontNback = DatabaseAccess.QAOfStacks(stackpicked);
            foreach (var kvp in frontNback)
            {
                Table table = new Table();
                table.Border = TableBorder.DoubleEdge;
                table.AddColumn("Front");
                table.AddRow(Markup.Escape(kvp.Key));
                AnsiConsole.Write(table);
                AnsiConsole.MarkupLine("[green]Answer[/] the following Question:");
                string answer = Console.ReadLine().Trim();
                if (answer == kvp.Value)
                {
                    correctcount++;
                    AnsiConsole.MarkupLine("[green]Correct[/] Answer!!! ");
                }
                totalcount++;
                AnsiConsole.MarkupLine("press [red]'x' [/] to exit \n any Key to continue");

                var userinp = Console.ReadLine().ToLower();
                if (userinp == "x")
                {
                    result = Math.Round((double)(((double)correctcount / totalcount) * 100));
                    return result;
                }
            }
            result = Math.Round((double)(((double)correctcount / totalcount) * 100));
            return result;
        }
        public static int ViewReportMenu()
        {
            AnsiConsole.MarkupLine("Please enter '1' to view Report for all the years:\n enter '2' to view report for specific year:");
            bool res = int.TryParse(Console.ReadLine(), out int userInput);
            while (!res || (userInput < 1 || userInput > 2))
            {
                AnsiConsole.MarkupLine("[Red] Wrong Choice[/] Please enter an integer between 1 and 2:");
                res = int.TryParse(Console.ReadLine(), out userInput);
            }
            return userInput;
        }
        public static int GetYear()
        {
            AnsiConsole.MarkupLine("Please enter the year in [yellow] yyyy [/] format to view the report");
            bool res = DateTime.TryParseExact(Console.ReadLine(), "yyyy", null, DateTimeStyles.None, out DateTime year);
            while (!res)
            {
                AnsiConsole.MarkupLine("[Red] Wrong Choice[/] Please enter year in [yellow]'yyyy'[/] format");
                res = DateTime.TryParseExact(Console.ReadLine(), "yyyy", null, DateTimeStyles.None, out year);
            }
            return int.Parse(year.ToString("yyyy"));
        }
    }
}
