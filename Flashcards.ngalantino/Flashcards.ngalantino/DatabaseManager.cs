using System.Data.SqlClient;
internal class DatabaseManager
{
    internal DatabaseManager()
    {
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {
            connection.Open();

            // Create database if not exists.
            String sql = @"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name='flashcardstest')
                                CREATE DATABASE flashcardstest";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

            // Create stacks table if not exists.
            sql = @"IF OBJECT_ID(N'flashcardstest.dbo.stacks', 'U') IS NULL
                        BEGIN
                            CREATE TABLE flashcardstest.dbo.stacks (
                                stack nvarchar(50) NOT NULL PRIMARY KEY
                            )
                        END";

            command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

            // Create flashcards table if not exists.
            sql = @"IF OBJECT_ID(N'flashcardstest.dbo.flashcards', 'U') IS NULL
                        BEGIN
                            CREATE TABLE flashcardstest.dbo.flashcards (
                            id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                            stack nvarchar(50),
                            front nvarchar(MAX),
                            back nvarchar(MAX),
                            constraint FK_flashcards_stacks FOREIGN KEY (stack) references flashcardstest.dbo.stacks(stack) ON DELETE CASCADE,
                        )
                        END";

            command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

            // Create studysessions table if not exists.
            sql = @"IF OBJECT_ID(N'flashcardstest.dbo.studysessions', 'U') IS NULL
                        BEGIN
                            CREATE TABLE flashcardstest.dbo.studysessions (
                            id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                            stack nvarchar(50),
                            date nvarchar(50),
                            score int,
                            constraint FK_study_sessions_stacks FOREIGN KEY (stack) references flashcardstest.dbo.stacks(stack) ON DELETE CASCADE,
                        )
                        END";

            command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

        }
    }

    public List<Flashcard> GetFlashcards(String stack, int numFlashcards = 0)
    {
        List<Flashcard> flashcards = new List<Flashcard>();

        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {
            connection.Open();

            String sql = $"SELECT * FROM flashcardstest.dbo.flashcards WHERE stack='{stack}'";

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

            String sql = $"SELECT * FROM flashcardstest.dbo.stacks";

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

            string sql = $"INSERT INTO flashcardstest.dbo.stacks (stack) VALUES ('{name}')";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }
    }

    public void DeleteStack(String stack)
    {
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {
            connection.Open();

            string sql = $"DELETE FROM flashcardstest.dbo.stacks WHERE stack='{stack}'";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }
    }

    public void AddFlashcard(Flashcard flashcard)
    {
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {

            connection.Open();

            string sql = $"INSERT INTO flashcardstest.dbo.flashcards (stack, front, back) VALUES('{flashcard.Stack}', '{flashcard.Front}', '{flashcard.Back}')";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();

        }
    }

    public void EditFlashcard(Flashcard flashcard)
    {
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {

            connection.Open();

            string sql = @$"UPDATE flashcardstest.dbo.flashcards
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

            string sql = $"DELETE FROM flashcardstest.dbo.flashcards WHERE id='{flashcard.Id}'";

            SqlCommand command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }
    }

    public void AddStudySession(String stack, String date, int score)
    {

        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("dbConnection")))
        {

            connection.Open();

            string sql = $"INSERT INTO flashcardstest.dbo.studysessions (stack, date, score) VALUES ('{stack}', '{date}', '{score}')";

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

            string sql = "SELECT * FROM flashcardstest.dbo.studysessions";

            SqlCommand command = new SqlCommand(sql, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                StudySession session = new StudySession
                {
                    Stack = reader.GetString(1),
                    //Date = reader.GetDateTime(2),
                    Date = reader.GetString(2),
                    Score = reader.GetInt32(3)

                };

                studySessions.Add(session);
            }

            return studySessions;
        }
    }

}

