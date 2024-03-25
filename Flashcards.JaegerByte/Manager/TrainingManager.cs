using Flashcards.JaegerByte.DatabaseHandler;
using Flashcards.JaegerByte.DataModels;
using Spectre.Console;

namespace Flashcards.JaegerByte.Manager
{
    internal class TrainingManager
    {
        public DatabaseTrainingHandler  dbHandler { get; set; }
        public CardStack SelectedStack { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public void Init(TrainingMenuOption option)
        {
            dbHandler = new DatabaseTrainingHandler();
            while(true)
            {
                switch (option)
                {
                    case TrainingMenuOption.StartNewSession:
                        ResetSession();
                        SelectStack();
                        StartTime = DateTime.Now;
                        while (true)
                        {
                            Console.Clear();
                            AskPrompt();
                            SelectionPrompt<string> selectionPrompt = new SelectionPrompt<string>();
                            selectionPrompt.AddChoice("Next Question");
                            selectionPrompt.AddChoice("Save and exit Session");
                            string selection = AnsiConsole.Prompt(selectionPrompt);
                            if (selection == "Save and exit Session")
                            {
                                EndTime = DateTime.Now;
                                SaveSession();
                                break;
                            }
                        }
                        break;
                    case TrainingMenuOption.Exit:
                        break;
                }
                break;
            }
        }

        private void ResetSession()
        {
            SelectedStack = null;
            CorrectAnswers = 0;
            WrongAnswers = 0;
        }

        private void SelectStack()
        {
            DatabaseStackHandler databaseStackHandler = new DatabaseStackHandler();
            List<CardStack> cardStacks = databaseStackHandler.GetAll();
            SelectionPrompt<CardStack> selectionprompt = new SelectionPrompt<CardStack>();
            selectionprompt.Title = "Select Stack to start new TrainingSession!";
            foreach (var item in cardStacks)
            {
                selectionprompt.AddChoice(item);
            }
            SelectedStack = AnsiConsole.Prompt(selectionprompt);
        }

        private int GetRandomFlashcardID()
        {
            int flashcardsInStack = SelectedStack.Flashcards.Count;
            Random random = new Random();
            return random.Next(0, flashcardsInStack);
        }

        private void AskPrompt()
        {
            AnsiConsole.WriteLine(SelectedStack.Title);
            AnsiConsole.WriteLine("Type in answer and confirm with ENTER\n");
            int randomFlashcardID = GetRandomFlashcardID();
            string answer = AnsiConsole.Ask<string>($"{SelectedStack.Flashcards[randomFlashcardID].Question}");
            if (answer == SelectedStack.Flashcards[randomFlashcardID].Answer)
            {
                AnsiConsole.WriteLine("Correct!");
                CorrectAnswers++;
            }
            else
            {
                AnsiConsole.WriteLine($"Wrong! The correct answer is {SelectedStack.Flashcards[randomFlashcardID].Answer}");
                WrongAnswers++;
            }
        }
        private void SaveSession()
        {
            TrainingSession newSession = new TrainingSession(SelectedStack.StackID, StartTime, EndTime, CorrectAnswers, WrongAnswers);
            dbHandler.InsertSession(newSession);
        }
    }
}
