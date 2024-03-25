using Flashcards.JaegerByte.DatabaseHandler;
using Flashcards.JaegerByte.DataModels;
using Spectre.Console;

namespace Flashcards.JaegerByte.Manager
{
    internal class FlashcardsManager
    {
        public DatabaseFlashcardHandler DbHandler { get; set; }
        public void Init(FlashcardsMenuOption option)
        {
            DbHandler = new DatabaseFlashcardHandler();
            while (true)
            {
                switch (option)
                {
                    case FlashcardsMenuOption.AddFlashcard:
                        AddFlashcard();
                        break;
                    case FlashcardsMenuOption.DeleteFlashcard:
                        DeleteFlashcard();
                        break;
                    case FlashcardsMenuOption.ViewAllFlashcards:
                        ViewFlashcard();
                        break;
                    case FlashcardsMenuOption.Exit:
                        break;
                }
                break;
            }
        }
        private void AddFlashcard()
        {
            string newQuestion = AnsiConsole.Ask<string>("Please insert new Question");
            string newAnswer = AnsiConsole.Ask<string>("Please insert new Answer");
            if (DbHandler.CheckInsertInput(newQuestion, newAnswer))
            {
                Flashcard newFlashcard = new Flashcard(newQuestion, newAnswer);
                DatabaseStackHandler databaseStackHandler = new DatabaseStackHandler();
                List<CardStack> cardStacks = databaseStackHandler.GetAll();
                SelectionPrompt<CardStack> selectionprompt = new SelectionPrompt<CardStack>();
                selectionprompt.Title = "Select Stack to add new Flashcard";
                foreach (var item in cardStacks)
                {
                    selectionprompt.AddChoice(item);
                }
                CardStack whereStack = AnsiConsole.Prompt(selectionprompt);

                DbHandler.Insert(newFlashcard, whereStack);
            }
            else
            {
                AnsiConsole.Write(DbHandler.GetInvalidResponse());
                Console.ReadKey();
            }
        }
        private void DeleteFlashcard()
        {
            List<Flashcard> flashcards = DbHandler.GetAll();
            SelectionPrompt<Flashcard> selectionprompt = new SelectionPrompt<Flashcard>();
            selectionprompt.Title = "Select Flashcard to delete";
            foreach (var item in flashcards)
            {
                selectionprompt.AddChoice(item);
            }
            Flashcard flashcardToDelete = AnsiConsole.Prompt(selectionprompt);
            DbHandler.Delete(flashcardToDelete);
        }
        private void ViewFlashcard()
        {
            List<Flashcard> flashcards = new List<Flashcard>();
            flashcards = DbHandler.GetAll();

            Grid grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddRow(new string[] { "FlashcardID", "Question", "Answer" });
            int id = 1;
            foreach (var item in flashcards)
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
