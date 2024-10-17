using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
internal class DatabaseManager
{
    internal DatabaseManager()
    {

    }

    // Return list of flashcard DTOs.
    // Add input parameter to return specific stack
    public List<Flashcard> GetFlashcards(String stack, int numFlashcards = 0)
    {
        List<Flashcard> flashcards = new List<Flashcard>();

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings.Get("dbConnection")))
        {
            connection.Open();

            String sql = $"SELECT * FROM flashcards WHERE stack='{stack}'";

            SqlCommand command = new SqlCommand(sql, connection);

            SqlDataReader reader = command.ExecuteReader();

            if (numFlashcards != 0)
            {
                while (reader.Read() && numFlashcards > 0)
                {
                    Flashcard flashcard = new Flashcard
                    {
                        id = reader.GetInt32(0),
                        stack = reader.GetString(1),
                        front = reader.GetString(2),
                        back = reader.GetString(3)
                    };

                    flashcards.Add(flashcard);
                    numFlashcards--;
                }
            }

            else
            {
                while (reader.Read())
                {
                    Flashcard flashcard = new Flashcard
                    {
                        id = reader.GetInt32(0),
                        stack = reader.GetString(1),
                        front = reader.GetString(2),
                        back = reader.GetString(3)
                    };

                    flashcards.Add(flashcard);
                }
            }

            connection.Close();
        }

        return flashcards;
    }

    public List<Stack> GetStacks()
    {
        List<Stack> stacks = new List<Stack>();
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings.Get("dbConnection")))
        {
            connection.Open();

            String sql = $"SELECT * FROM stacks";

            SqlCommand command = new SqlCommand(sql, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Stack stack = new Stack
                {
                    name = reader.GetString(0)
                };
                stacks.Add(stack);
            }
            connection.Close();
        }
        return stacks;

    }

    // Add flashcard to stack
    public void AddFlashcard(Flashcard flashcard)
    {
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings.Get("dbConnection")))
        {

            // TODO: Create parameterized query

            connection.Open();

            string sql = $"INSERT INTO flashcards (stack, front, back) VALUES('{flashcard.stack}', '{flashcard.front}', '{flashcard.back}')";

            SqlCommand command = new SqlCommand(sql, connection);

            Int32 recordsAffected = command.ExecuteNonQuery();

        }
    }

    public void EditFlashcard(Flashcard flashcard)
    {
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings.Get("dbConnection")))
        {

            connection.Open();

            string sql = @$"UPDATE flashcards
                            SET front = '{flashcard.front}', back = '{flashcard.back}'
                            WHERE id = '{flashcard.id}'";

            SqlCommand command = new SqlCommand(sql, connection);

            Int32 recordsAffected = command.ExecuteNonQuery();

        }
    }

    public void DeleteFlashcard(Flashcard flashcard)
    {
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings.Get("dbConnection")))
        {
            connection.Open();

            string sql = $"DELETE FROM flashcards WHERE id='{flashcard.id}'";

            SqlCommand command = new SqlCommand(sql, connection);

            Int32 recordsAffected = command.ExecuteNonQuery();
        }
    }

}

