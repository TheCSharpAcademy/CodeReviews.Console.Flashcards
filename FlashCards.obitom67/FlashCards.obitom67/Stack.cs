using System;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using Spectre.Console;

namespace FlashCards.obitom67
{
    public class Stack
    {

        public string StackName { get; set; }
        public int StackId { get; set; }

        
        public static void CreateStack()
        {
            Stack stack = new Stack();
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            
            stack.StackName = AnsiConsole.Ask<string>("Please input a name for this stack.");
            
            using (var connection = new SqlConnection(connectionString))
            {
                string count = $"SELECT * FROM dbo.Stack";
                connection.Open();
                var stacks = connection.Query(count);
                int stackId;
                var sortStack = stacks.OrderBy(s => s.StackId);
                if (sortStack.Count() == 0)
                {
                    stackId = 1;
                }
                else
                {
                    stackId = sortStack.Last().StackId+1;
                }
                string sql = $"INSERT INTO dbo.Stack (StackName,StackId) VALUES ('{stack.StackName}',{stackId})";
                connection.Execute(sql);
            }
            

        }

        public static void ReadStack(Stack stackToRead)
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            using(var connection = new SqlConnection(connectionString))
            {
                
                string flashcardSql = $"SELECT * FROM dbo.Flashcard WHERE StackId = {stackToRead.StackId}";
                var flashcardSelect = connection.Query(flashcardSql);
                var sortSelect = flashcardSelect.OrderBy(f => f.FlashcardId);

                foreach(var flashcard in sortSelect)
                {
                    AnsiConsole.WriteLine($"{flashcard.FlashcardId} |{flashcard.FrontText} | {flashcard.BackText}");
                }
                
            }
            
        }

        public static void UpdateStack()
        {

        }
        public static void DeleteStack(Stack stack)
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            using(var connection = new SqlConnection(connectionString))
            {
                string deleteCards = $"DELETE FROM dbo.Flashcard WHERE StackId = {stack.StackId}";
                connection.Execute(deleteCards);
                string deleteStack = $"DELETE FROM dbo.Stack WHERE StackId = {stack.StackId}"; 
                connection.Execute(deleteStack);
                string deleteSessions = $"DELETE FROM dbo.StudySessions WHERE StackId = {stack.StackId}";

            }
        }

        public static Stack DisplayStacks()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM dbo.Stack";
                List<Stack> stacks = new List<Stack>();
                List<string> stackNames = new List<string>();
                var stackSelect = connection.Query(sql);

                foreach (var stack in stackSelect)
                {
                    Stack tempStack = new Stack();
                    tempStack.StackId = stack.StackId;
                    tempStack.StackName = stack.StackName;
                    stackNames.Add(tempStack.StackName);
                    stacks.Add(tempStack);
                }

                SelectionPrompt<string> selectionPrompt = new SelectionPrompt<string>();
                selectionPrompt.AddChoices(stackNames);
                selectionPrompt.Title = "Please Select a Stack to change:";
                
                var stackChoice = AnsiConsole.Prompt(selectionPrompt);
                var currentStack = stacks.First(stack => stack.StackName == stackChoice);
                
                return currentStack;
            }
            
        }

    }
}
