using Dapper;
using Flashcards.JaegerByte.DataModels;
using System.Data.SqlClient;

namespace Flashcards.JaegerByte.DatabaseHandler
{
    internal class DatabaseStackHandler
    {
        public int MaxChars { get; set; } = 30;
        public int MinChars { get; set; } = 3;
        public string? InvalidResponse { get; set; }
        public List<CardStack> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
            {
                // zuerst alle flashcards ziehen und dann objekte erstellen
                connection.Open();
                List<CardStack> stacks = connection.Query<CardStack>("SELECT * FROM tblStacks").ToList<CardStack>();
                foreach (CardStack stack in stacks)
                {
                    stack.Flashcards = connection.Query<Flashcard>($"SELECT * FROM tblFlashcards WHERE StackID = {stack.StackID}").ToList<Flashcard>();
                }
                connection.Close();
                return stacks;
            }
        }
        public void Insert(CardStack stack)
        {
            using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
            {
                connection.Open();
                connection.Execute($"INSERT INTO tblStacks(Title) VALUES ('{stack.Title}')");
                connection.Close();
            }
        }
        public void Delete(CardStack stack)
        {
            using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
            {
                connection.Open();
                connection.Execute($"DELETE FROM tblTrainingSessions WHERE StackID = {stack.StackID}");
                connection.Execute($"DELETE FROM tblFlashcards WHERE StackID = '{stack.StackID}'");
                connection.Execute($"DELETE FROM tblStacks WHERE StackID ='{stack.StackID}'");
                connection.Close();
            }
        }
        public bool CheckInsertInput(string stackname)
        {
            if (stackname.Length>MaxChars)
            {
                InvalidResponse = $"invalid input. Stackname too long (max {MaxChars} chars).\nPress ANY key to return";
                return false;
            }
            if (stackname.Length<MinChars)
            {
                InvalidResponse = $"invalid input. Stackname too short (min {MinChars} chars).\nPress ANY key to return";
                return false;
            }
            return true;

        }
        public string GetInvalidResponse()
        {
            return InvalidResponse;
        }
    }
}
