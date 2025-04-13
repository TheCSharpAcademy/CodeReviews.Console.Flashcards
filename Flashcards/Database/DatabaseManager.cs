
using Microsoft.Data.SqlClient;
using Flashcards.Validation;
using Flashcards.Object_Classes;
using System.Globalization;
namespace Flashcards.Database
{
    public static class DatabaseManager
    {
        static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");
        static string databaseName = System.Configuration.ConfigurationManager.AppSettings.Get("DatabaseName");
        static string flashcardTable = System.Configuration.ConfigurationManager.AppSettings.Get("FlashcardTable");
        static string stackTable = System.Configuration.ConfigurationManager.AppSettings.Get("StackTable");
        static string studyTable = System.Configuration.ConfigurationManager.AppSettings.Get("StudyTable");
        public static void InitializeDatabase()
        {
            string dbConnectionString = connectionString + ";Trusted_Connection=True;";

            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"
                    IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{databaseName}')
                    BEGIN
                    CREATE DATABASE {databaseName}
                    END";

                tableCmd.ExecuteNonQuery();
            }

            CreateTables();
        }

        public static void CreateTables()
        {
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";

            
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                //Create Stack table
                //Each stack must have a unique name so when we look them up we get the correct one
                tableCmd.CommandText =
                    @$"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{stackTable}' AND xtype='U')
                    BEGIN
                        CREATE TABLE {stackTable} (
                            StackID INT PRIMARY KEY IDENTITY(1,1),
                            StackName NVARCHAR(100) NOT NULL UNIQUE
                        )
                    END";

                tableCmd.ExecuteNonQuery();

                //Create Flashcard Table with foreign key to Stack Table
                //On the deletion of a Stack, all flashcards in that stack will be deleted
                tableCmd.CommandText =
                    @$"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{flashcardTable}' AND xtype='U')
                    BEGIN
                        CREATE TABLE {flashcardTable} (
                            FlashcardID INT PRIMARY KEY IDENTITY(1,1),
                            StackID INT NOT NULL,
                            Question NVARCHAR(MAX) NOT NULL,
                            Answer NVARCHAR(MAX) NOT NULL,
                            CONSTRAINT FK_{flashcardTable}_{stackTable} FOREIGN KEY (StackId)
                            REFERENCES {stackTable}(StackId) ON DELETE CASCADE
                        )
                    END";

                tableCmd.ExecuteNonQuery();

                //Create Study Session Table with foreign key to Stack Table
                //On the deletion of a Stack, all Study Session records attached to that Stack are deleted
                tableCmd.CommandText =
                    @$"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{studyTable}' AND xtype='U')
                    BEGIN
                        CREATE TABLE {studyTable} (
                            StudyID INT PRIMARY KEY IDENTITY(1,1),
                            StackName NVARCHAR(100) NOT NULL,           
                            StackID INT NOT NULL,
                            Score INT NOT NULL,
                            TotalQuestions INT NOT NULL,
                            SessionDate TEXT NOT NULL,
                            SessionDuration INT NOT NULL,
                            CONSTRAINT FK_{studyTable}_{stackTable} FOREIGN KEY (StackId)
                            REFERENCES {stackTable}(StackId) ON DELETE CASCADE
                        )
                    END";

                tableCmd.ExecuteNonQuery();
            }
        }

        public static void CreateStack(string stackName)
        {
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";


            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"
                    INSERT INTO {stackTable} (StackName)
                    VALUES ('{stackName}')
                    ";

                tableCmd.ExecuteNonQuery();
            }
        }

        public static void UpdateStack(string stackName, int stackID)
        {
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";

            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"
                    UPDATE {stackTable}
                    SET StackName = '{stackName}'
                    WHERE StackID = {stackID}
                    ";

                tableCmd.ExecuteNonQuery();
            }
        }

        public static void DeleteStack(int stackID)
        {
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";

            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"
                    DELETE FROM {stackTable}
                    WHERE StackID = {stackID}
                    ";

                tableCmd.ExecuteNonQuery();
            }
        }

        public static List<Stack> GetFlashcardStacks(int specificStack = -1)
        {
            List<Stack> stacks = new();
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";

            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                if (specificStack == -1)
                {
                    tableCmd.CommandText = @$"SELECT StackID, StackName FROM {stackTable} ORDER BY 1 ASC";
                }
                else
                {
                    tableCmd.CommandText = @$"SELECT StackID, StackName FROM {stackTable} WHERE StackID = {specificStack} ORDER BY 1 ASC";
                }

                SqlDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    Stack stack = new Stack
                    {
                        StackId = reader.GetInt32(reader.GetOrdinal("StackID")),
                        StackName = reader.GetString(reader.GetOrdinal("StackName"))
                    };

                    stacks.Add(stack);
                }
            }
            return stacks;
        }

        public static string GetStackName(int stackID)
        {
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";

            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @$"SELECT StackName FROM {stackTable} WHERE StackID = {stackID} ORDER BY 1 ASC";
                

                SqlDataReader reader = tableCmd.ExecuteReader();

                reader.Read();
                return reader.GetString(reader.GetOrdinal("StackName"));
            }
        }

        public static void CreateFlashcard(string question, string answer, int stackID)
        {
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";

            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"
                    INSERT INTO {flashcardTable} (StackID, Question, Answer)
                    VALUES ({stackID}, '{question}','{answer}')
                    ";

                tableCmd.ExecuteNonQuery();
            }
        }

        public static void UpdateFlashcard(string question, string answer, int flashcardID)
        {
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";

            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"
                    UPDATE {flashcardTable}
                    SET Question = '{question}', Answer = '{answer}'
                    WHERE FlashcardID = {flashcardID}
                    ";

                tableCmd.ExecuteNonQuery();
            }
        }

        public static void DeleteFlashcard(int flashcardID)
        {
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";

            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"
                    DELETE FROM {flashcardTable}
                    WHERE FlashcardID = {flashcardID}
                    ";

                tableCmd.ExecuteNonQuery();
            }
        }

        public static List<FlashcardDTO> GetFlashcards(int specificStack = -1)
        {
            List<FlashcardDTO> flashcards = new();
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";

            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                if(specificStack == -1)
                {
                       tableCmd.CommandText = @$"SELECT FlashcardId, Question, Answer FROM {flashcardTable} ORDER BY 1 ASC"; 
                }
                else
                {
                    tableCmd.CommandText = @$"SELECT FlashcardId, Question, Answer FROM {flashcardTable} WHERE StackID = {specificStack} ORDER BY 1 ASC";
                }

                SqlDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    FlashcardDTO flashcard = new FlashcardDTO
                    {
                        FlashcardId = reader.GetInt32(reader.GetOrdinal("FlashcardId")),
                        Question = reader.GetString(reader.GetOrdinal("Question")),
                        Answer = reader.GetString(reader.GetOrdinal("Answer"))
                    };

                    flashcards.Add(flashcard);
                }
            }          
            return flashcards;
        }

        public static void CreateStudySession(int score, int totalQuestions, int stackID, string stackName, DateTime startTime, int duration)
        {
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";
            string sessionDate = startTime.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"
                    INSERT INTO {studyTable} (StackID, StackName, Score, TotalQuestions, SessionDate, SessionDuration)
                    VALUES ({stackID}, '{stackName}', {score},{totalQuestions}, '{sessionDate}', {duration})
                    ";

                tableCmd.ExecuteNonQuery();
            }
        }

        public static List<StudySessionDTO> GetStudySessions(int specificStack = -1)
        {
            List<StudySessionDTO> studySessions = new();
            string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";

            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                if (specificStack == -1)
                {
                    tableCmd.CommandText = @$"SELECT * FROM {studyTable} ORDER BY 1 ASC";
                }
                else
                {
                    tableCmd.CommandText = @$"SELECT * FROM {studyTable} WHERE StudyID = {specificStack} ORDER BY 1 ASC";
                }

                SqlDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    StudySessionDTO studySession = new StudySessionDTO
                    {
                        StudySessionId = reader.GetInt32(reader.GetOrdinal("StudyID")),
                        StackName = reader.GetString(reader.GetOrdinal("StackName")),
                        Score = reader.GetInt32(reader.GetOrdinal("Score")),
                        TotalQuestions = reader.GetInt32(reader.GetOrdinal("TotalQuestions")),
                        SessionDate = reader.GetString(reader.GetOrdinal("SessionDate")),
                        SessionDuration = reader.GetInt32(reader.GetOrdinal("SessionDuration"))
                    };

                    studySessions.Add(studySession);
                }
            }
            return studySessions;
        }

        public static bool DoWeHaveFlashcardStacks()
        {
            return DatabaseValidator.DoWeHaveRows(stackTable);
        }
        public static bool DoWeHaveFlashCards()
        {
            return DatabaseValidator.DoWeHaveRows(flashcardTable);
        }
        public static bool DoWeHaveStudySessions()
        {
            return DatabaseValidator.DoWeHaveRows(studyTable);
        }
    }
}
