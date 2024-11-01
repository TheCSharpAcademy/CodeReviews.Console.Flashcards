using System.Data.SqlClient;
internal class DatabaseManager
{
    internal DatabaseManager()
    {

    }

    public List<Flashcard> GetFlashcards(String stack, int numFlashcards = 0)
    {
        List<Flashcard> flashcards = new List<Flashcard>();

        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
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
                        Id = reader.GetInt32(0),
                        Stack = reader.GetString(1),
                        Front = reader.GetString(2),
                        Back = reader.GetString(3)
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
                        Id = reader.GetInt32(0),
                        Stack = reader.GetString(1),
                        Front = reader.GetString(2),
                        Back = reader.GetString(3)
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
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {
            connection.Open();

            String sql = $"SELECT * FROM stacks";

            SqlCommand command = new SqlCommand(sql, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Stack stack = new Stack
                {
                    Name = reader.GetString(0)
                };
                stacks.Add(stack);
            }
            connection.Close();
        }
        return stacks;

    }

    public void NewStack(String name)
    {
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {
            connection.Open();

            string sql = $"INSERT INTO stacks (stack) VALUES ('{name}')";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }
    }

    public void DeleteStack(String stack)
    {
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {
            connection.Open();

            string sql = $"DELETE FROM stacks WHERE stack='{stack}'";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }
    }

    public void AddFlashcard(Flashcard flashcard)
    {
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {

            connection.Open();

            string sql = $"INSERT INTO flashcards (stack, front, back) VALUES('{flashcard.Stack}', '{flashcard.Front}', '{flashcard.Back}')";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

        }
    }

    public void EditFlashcard(Flashcard flashcard)
    {
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {

            connection.Open();

            string sql = @$"UPDATE flashcards
                            SET front = '{flashcard.Front}', back = '{flashcard.Back}'
                            WHERE id = '{flashcard.Id}'";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

        }
    }

    public void DeleteFlashcard(Flashcard flashcard)
    {
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {
            connection.Open();

            string sql = $"DELETE FROM flashcards WHERE id='{flashcard.Id}'";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }
    }

    public void AddStudySession(String stack, DateTime date, int score)
    {

        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {

            connection.Open();

            string sql = $"INSERT INTO studysessions (stack, date, score) VALUES ('{stack}', '{date}', '{score}')";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }
    }

    public List<StudySession> GetStudySessions()
    {
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {

            List<StudySession> studySessions = new List<StudySession>();

            connection.Open();

            string sql = "SELECT * FROM studysessions";

            SqlCommand command = new SqlCommand(sql, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                StudySession session = new StudySession
                {
                    Stack = reader.GetString(1),
                    Date = reader.GetDateTime(2),
                    Score = reader.GetInt32(3)

                };

                studySessions.Add(session);
            }

            return studySessions;
        }
    }

}

