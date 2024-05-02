using Spectre.Console;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace FlashCards.obitom67
{
    public class FlashCard
    {
        int FlashCardId { get; set; }
        string FrontText {  get; set; }
        string BackText { get; set; }
        int StackId {  get; set; }


        public static void CreateFlashCard(Stack stack)
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            FlashCard flashCard = new FlashCard();
            flashCard.FrontText = AnsiConsole.Ask<string>("Please input the front text of the flashcard.");
            flashCard.BackText = AnsiConsole.Ask<string>("Please input the back text of the flashcard.");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string count = $"SELECT COUNT(*) FROM dbo.Flashcard WHERE StackId = {stack.StackId}";
                var flashcardCount = connection.ExecuteScalar(count);
                string sql = $"INSERT INTO Flashcard(FrontText,BackText,FlashcardId,StackId) VALUES ('{flashCard.FrontText}','{flashCard.BackText}',{(int)flashcardCount + 1},{stack.StackId})";
                connection.Execute(sql);
            }
        }

        public static void UpdateFlashCards(Stack stack)
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            using(var connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM dbo.Flashcard WHERE StackId = {stack.StackId}";
                var flashcardSelect = connection.Query(sql);
                List<FlashCard> flashcards = new List<FlashCard>();
                

                foreach (var flashcard in flashcardSelect) 
                {
                    FlashCard temp = new FlashCard();
                    temp.StackId = flashcard.StackId;
                    temp.FlashCardId = flashcard.FlashcardId;
                    temp.FrontText = flashcard.FrontText;
                    temp.BackText = flashcard.BackText;
                    flashcards.Add(temp);
                }
                Stack.ReadStack(stack);
                int updateId = AnsiConsole.Ask<int>("Please enter the FlashcardId you would like to update:");
                FlashCard flashcardChange = flashcards.First(f  => f.FlashCardId == updateId);
                flashcardChange.FrontText = AnsiConsole.Ask<string>("Please input the front text of the flashcard.");
                flashcardChange.BackText = AnsiConsole.Ask<string>("Please input the back text of the flashcard.");

                string updateSql = $"UPDATE dbo.Flashcard SET FrontText = '{flashcardChange.FrontText}',BackText = '{flashcardChange.BackText}' WHERE FlashcardId = {updateId}";
                connection.Execute(updateSql);
            }
        }

        public static void DeleteFlashCard(Stack stack)
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            

            using (var connection = new SqlConnection(connectionString))
            {
                AnsiConsole.Clear();
                Stack.ReadStack(stack);
                int deleteId = AnsiConsole.Ask<int>("Please enter the FlashcardId you would like to delete:");
                string sql = $"DELETE FROM dbo.Flashcard WHERE FlashcardId = {deleteId}";
                connection.Execute(sql);
                string sqlCount = $"SELECT * FROM dbo.Flashcard WHERE StackId = {stack.StackId}";
                var flashcards = connection.Query(sqlCount);
                int count = 1;
                foreach (var flashcard in flashcards)
                {
                    string updateId = $"UPDATE dbo.Flashcard SET FlashcardId = {count} WHERE FlashcardId = {flashcard.FlashcardId}";
                    connection.Execute(updateId);
                    count++;
                }

            }
        }
    }
}
