using Flashcards.Study.Models;
using Flashcards.Study.Models.Domain;
using Spectre.Console;
using System.Reflection;
namespace Flashcards.Study
{
    public class UserOutputs
    {
        public static void ManageStacks()
        {
            List<string> stacks = new List<string>();
            stacks = DatabaseAccess.GetAllStacks();
            Table stacktable = new Table();
            TableTitle title = new TableTitle("Stack Management Area");
            stacktable.Title = title;
            stacktable.AddColumn("The available stacks");
            stacks.ForEach(x => stacktable.AddRow(x));
            AnsiConsole.Write(stacktable);
            int userChoice = UserInputs.ManageStacksMenu();
            while (userChoice != 0)
            {
                if (userChoice == 1)
                {
                    Console.WriteLine("Please enter the name of the stack to enter");
                    string newstackname = Console.ReadLine();
                    var exists = stacks.FirstOrDefault(x => x == newstackname);
                    if (exists == null)
                    {
                        DatabaseAccess.InsertNewStack(newstackname);
                        AnsiConsole.MarkupLine($"[green]The Stack [yellow]{newstackname}[/] is added successfully[/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"The entered [yellow] {newstackname}[/] [red] already exists in the stack [/] ");
                    }
                }
                else if (userChoice == 2)
                {
                    Console.WriteLine("Please enter the name of the stack to delete:");
                    string name = Console.ReadLine();
                    DatabaseAccess.DeleteStack(name);
                    AnsiConsole.MarkupLine($"[red]The Stack [yellow]{name}[/] is  successfully[/]");
                }
                else if (userChoice == 3)
                {
                    var stackpicked = UserInputs.PickSingleStack(stacks);
                    int singlestackchoice = UserInputs.ManageSingleStack(stackpicked);
                    while (singlestackchoice != 0)
                    {
                        switch (singlestackchoice)
                        {
                            case 1:
                                stackpicked = UserInputs.PickSingleStack(stacks);
                                break;
                            case 2:
                                var result = DatabaseAccess.GetAllFlashcardsofStack(stackpicked);
                                ShowAllFlashcardsofStack(stackpicked, result);
                                break;
                            case 3:
                                DatabaseAccess.CreateNewFlashcardforstack(stackpicked);
                                ShowAllFlashcardsofStack(stackpicked, DatabaseAccess.GetAllFlashcardsofStack(stackpicked));
                                break;
                            case 4:
                                DatabaseAccess.DeleteFlashcardforstack(stackpicked, DatabaseAccess.GetAllFlashcardsofStack(stackpicked));
                                break;
                            case 5:
                                DatabaseAccess.EditFlashcardforstack(stackpicked, DatabaseAccess.GetAllFlashcardsofStack(stackpicked));
                                break;
                            default:
                                continue;
                                break;
                        }
                        singlestackchoice = UserInputs.ManageSingleStack(stackpicked);
                    }
                }
                userChoice = UserInputs.ManageStacksMenu();
            }
        }
        public static void ManageFlashcards()
        {
            Table flashcardsTable = new Table();
            flashcardsTable.Title = new TableTitle("Flashcards Management Area");
            flashcardsTable.Border = TableBorder.HeavyEdge;
            List<FlashcardDto> flashcards = new List<FlashcardDto>();
            flashcards = DatabaseAccess.GetAllFlashcards();
            var props = typeof(FlashcardDto).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                flashcardsTable.AddColumn(prop.Name);
            }
            flashcards.ForEach(x => flashcardsTable.AddRow(Markup.Escape(x.ID.ToString()), Markup.Escape(x.Front.ToString()), Markup.Escape(x.Back.ToString())));
            AnsiConsole.Write(flashcardsTable);
            int userChoice = UserInputs.ManageFlashcardsMenu();
            while (userChoice != 0)
            {
                if (userChoice == 1)
                {
                    Console.WriteLine("Please enter the ID of the flashcard to Delete:");
                    int id = UserInputs.DeleteFlashcardMenu(flashcards.Count);
                    DatabaseAccess.DeleteFlashcard(id);
                    AnsiConsole.MarkupLine($"[red]The Flashcard with ID[yellow]{id}[/] is deleted successfully[/]");
                }
                userChoice = UserInputs.ManageStacksMenu();
            }

        }
        public static void ShowAllFlashcardsofStack(string stackpicked, List<FlashcardDto> result)
        {
            Table flashcardtable = new Table();
            flashcardtable.Title = new TableTitle($"Flashcards of Stack {stackpicked}");
            flashcardtable.Border = TableBorder.HeavyEdge;
            var props = typeof(FlashcardDto).GetProperties();
            foreach (var prop in props)
            {
                flashcardtable.AddColumn(prop.Name);
            }
            result.ForEach(x => flashcardtable.AddRow(Markup.Escape(x.ID.ToString()), Markup.Escape(x.Front.ToString()), Markup.Escape(x.Back.ToString())));
            AnsiConsole.Write(flashcardtable);
        }
        public static void StudyArea()
        {
            Table stacktable = new Table();
            stacktable.Title = new TableTitle("Stacks for Study");
            stacktable.Border = TableBorder.HeavyEdge;
            List<string> stacks = DatabaseAccess.GetAllStacks();
            stacktable.AddColumn("StackName");
            stacks.ForEach(x => stacktable.AddRow(x));
            string stackpicked = UserInputs.PickSingleStack(stacks);
            double passpercentage = UserInputs.StudyAStack(stackpicked);
            DatabaseAccess.LogStudySession(passpercentage, stackpicked);
        }
        public static void ViewStudySessionReport()
        {
            int userChoice = UserInputs.ViewReportMenu();
            switch (userChoice)
            {
                case 1:
                    DatabaseAccess.GetAllReport();
                    break;
                case 2:
                    int year = UserInputs.GetYear();
                    DatabaseAccess.GetYearReport(year);
                    break;
            }
        }
    }
}
