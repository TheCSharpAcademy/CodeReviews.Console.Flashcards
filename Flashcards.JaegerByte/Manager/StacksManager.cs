using Flashcards.JaegerByte.DatabaseHandler;
using Flashcards.JaegerByte.DataModels;
using Spectre.Console;

namespace Flashcards.JaegerByte.Manager
{
    internal class StacksManager
    {
        public DatabaseStackHandler dbhandler { get; set; }
        public void Init(StacksMenuOption option)
        {
            dbhandler = new DatabaseStackHandler();
            while (true)
            {
                switch (option)
                {
                    case StacksMenuOption.AddStack:
                        AddStack();
                        break;
                    case StacksMenuOption.DeleteStack:
                        DeleteStack();
                        break;
                    case StacksMenuOption.ViewStack:
                        ViewStack();
                        break;
                    case StacksMenuOption.Exit:
                        break;
                }
                break;
            }
        }

        private void AddStack()
        {
            string newStackName = AnsiConsole.Ask<string>($"Insert new stack name ({dbhandler.MinChars} to {dbhandler.MaxChars} chars)");
            CardStack newStack = new CardStack(newStackName);
            if (dbhandler.CheckInsertInput(newStackName))
            {
                dbhandler.Insert(newStack);
            }
            else
            {
                Console.WriteLine(dbhandler.GetInvalidResponse());
                Console.ReadKey();
            }
        }

        private void DeleteStack()
        {
            List<CardStack> cardStacks = dbhandler.GetAll();
            SelectionPrompt<CardStack> selectionprompt = new SelectionPrompt<CardStack>();
            selectionprompt.Title = "Select Stack to delete";
            foreach (var item in cardStacks)
            {
                selectionprompt.AddChoice(item);
            }
            CardStack stackToDelete =  AnsiConsole.Prompt(selectionprompt);
            dbhandler.Delete(stackToDelete);
        }

        private void ViewStack()
        {
            List<CardStack> cardStacks = dbhandler.GetAll();
            SelectionPrompt<CardStack> selectionprompt = new SelectionPrompt<CardStack>();
            selectionprompt.Title = "Select Stack to view";
            foreach (var item in cardStacks)
            {
                selectionprompt.AddChoice(item);
            }
            CardStack stackToView = AnsiConsole.Prompt(selectionprompt);
            Grid grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddRow(new string[] { "FlashcardID", "Question", "Answer" });
            int id = 1;
            foreach (var item in stackToView.Flashcards)
            {
                grid.AddRow(new string[] { $"{id}", item.Question, item.Answer });
                id++;
            }
            AnsiConsole.Write(grid);
            AnsiConsole.Write("press ANY key to return");
            Console.ReadKey(true);
        }
    }
}
