using Spectre.Console;
using System.Data.SqlClient;
using Dapper;

namespace FlashCards.obitom67
{
    public class Display
    {

        public static bool CloseApplication { get; set; }

        public static void GetUserInput()
        {
            CloseApplication = false;
            string[] selectionChoices =
            {
                "Exit",
                "Manage Stacks",
                "Create Stack",
                "Study",
                "View Study Session Data"
            };
            SelectionPrompt<string> menuPrompt = new SelectionPrompt<string>();
            menuPrompt.AddChoices(selectionChoices);
            menuPrompt.Title = "Please select an option:";
            var menuSelection = AnsiConsole.Prompt<string>(menuPrompt);

            switch (menuSelection)
            {
                case "Exit":
                    CloseApplication = true;
                    AnsiConsole.WriteLine("Thank you for using The FlashCard App (TM)");
                    break;
                case "Manage Stacks":
                    ManageStacks();
                    break;
                case "Create Stack":
                    Stack.CreateStack();
                    break;
                case "Study":
                    StudySession.StartSession();
                    break;
                case "View Study Session Data":
                    StudySession.ShowRecords();
                    break;
            }

            
        }

        public static void ManageStacks()
        {
            Stack currentStack = new Stack();
            string[] stackSelections =
                    {
                        "Main Menu",
                        "Change Stack",
                        "View Flashcards",
                        "Create Flashcard",
                        "Update Flashcard",
                        "Delete Flashcard",
                        "Delete Stack",
                        "Change Stack Name"
                    };
            currentStack = Stack.DisplayStacks();
            
            SelectionPrompt<string> stackPrompt = new SelectionPrompt<string>();
            stackPrompt.AddChoices(stackSelections);
            stackPrompt.Title = "Please make a selection:";
            var stackSelection = AnsiConsole.Prompt(stackPrompt);
            switch (stackSelection)
            {
                case "Main Menu":
                    AnsiConsole.Clear();
                    GetUserInput();
                    break;
                case "Change Stack":
                    AnsiConsole.Clear();
                    ManageStacks();
                    break;
                case "View Flashcards":
                    AnsiConsole.Clear();
                    Stack.ReadStack(currentStack);
                    break;
                case "Create Flashcard":
                    AnsiConsole.Clear();
                    FlashCard.CreateFlashCard(currentStack);
                    break;
                case "Update Flashcard":
                    AnsiConsole.Clear();
                    FlashCard.UpdateFlashCards(currentStack);
                    break;
                case "Delete Flashcard":
                    AnsiConsole.Clear();
                    FlashCard.DeleteFlashCard(currentStack);
                    break;
                case "Delete Stack":
                    AnsiConsole.Clear();
                    Stack.DeleteStack(currentStack);
                    break;
                case "Change Stack Name":
                    AnsiConsole.Clear();
                    Stack.UpdateStack(currentStack);                    
                    break;
            }
        }

        public static void CreateDatabase()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("connectionString");
            using (var connection = new SqlConnection(connectionString))
            {
                string dbCheck = "SELECT DB_ID('FlashCards')";
                var dbNumber = connection.ExecuteScalar(dbCheck);

                if(dbNumber == null )
                {
                    string dbCreate = "CREATE DATABASE FlashCards";
                    string flashcardTable = "CREATE TABLE FlashCards.dbo.Flashcard(" +
                        "FlashcardId int NOT NULL," +
                        "FrontText varchar(50) NOT NULL," +
                        "BackText varchar(50) NOT NULL," +
                        "StackId int NOT NULL," +
                        "FOREIGN KEY(StackId) REFERENCES FlashCards.dbo.Stack(StackId))";
                    string stackTable = "CREATE TABLE  FlashCards.dbo.Stack(" +
                        "StackId int NOT NULL PRIMARY KEY," +
                        "StackName varchar(50) NOT NULL)";
                    string seshTable = "CREATE TABLE FlashCards.dbo.StudySessions(" +
                        "StudyId int NOT NULL PRIMARY KEY," +
                        "StackId int NOT NULL REFERENCES FlashCards.dbo.Stack(StackId)," +
                        "CorrectQ int NOT NULL," +
                        "TotalQ int NOT NULL," +
                        "Date varchar(50) NOT NULL," +
                        "FOREIGN KEY(StackId) REFERENCES FlashCards.dbo.Stack(StackId))";
                    connection.Execute(dbCreate);
                    connection.Execute(stackTable);
                    connection.Execute(flashcardTable);             
                    connection.Execute(seshTable);
                }


            }
        }
    }
}
