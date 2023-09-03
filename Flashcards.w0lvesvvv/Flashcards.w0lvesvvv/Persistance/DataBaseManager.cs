using Flashcards.w0lvesvvv.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Flashcards.w0lvesvvv.Persistance
{
    public static class DataBaseManager
    {
        private static readonly SqlConnection sql = new SqlConnection(ConfigurationManager.AppSettings["sqlConnectionString"]);
        private static readonly SqlConnection db = new SqlConnection(ConfigurationManager.AppSettings["dbConnectionString"]);

        #region DataBase
        public static void CreateDatabase()
        {
            string query = $@"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Flashcard')
                              BEGIN
                                  CREATE DATABASE Flashcard;
                              END;";

            SqlCommand command = new SqlCommand(query, sql);

            sql.Open();
            command.ExecuteNonQuery();
            sql.Close();
        }

        public static void CreateTables()
        {
            string query = $@"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stack')
                              BEGIN
                                  CREATE TABLE Stack (
                                        StackId                 INT             NOT NULL IDENTITY PRIMARY KEY
                                      , StackName               NVARCHAR(250)   NOT NULL
                                  );
                              END

                              IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Card')
                              BEGIN
                                  CREATE TABLE Card (
                                        CardId                  INT             NOT NULL IDENTITY PRIMARY KEY
                                      , CardStackId             INT             NOT NULL 
                                      , CardQuestion           NVARCHAR(250)   NOT NULL
                                      , CardAnswer              NVARCHAR(250)   NOT NULL
                                      , FOREIGN KEY (CardStackId) REFERENCES Stack(StackId) ON DELETE CASCADE
                                  );
                              END

                              IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySession')
                              BEGIN
                                  CREATE TABLE StudySession (
                                        StudySessionId          INT             NOT NULL IDENTITY PRIMARY KEY
                                      , StudySessionStackId     INT             NOT NULL 
                                      , StudySessionDate        DATETIME        NOT NULL
                                      , StudySessionPoints      INT             NOT NULL
                                      , StudySessionMaxPoints   INT             NOT NULL
                                      , FOREIGN KEY (StudySessionStackId) REFERENCES Stack(StackId) ON DELETE CASCADE
                                  );
                              END";

            SqlCommand command = new SqlCommand(query, db);

            db.Open();
            command.ExecuteNonQuery();
            db.Close();
        }
        #endregion

        #region Stack
        public static bool InsertStack(string stackName)
        {
            bool result = false;

            string query = $@"IF EXISTS (SELECT TOP 1 1 FROM Stack WHERE StackName = @stackName)
                              BEGIN
                                  RETURN;
                              END

                              INSERT INTO Stack (StackName) VALUES (@stackName);";

            SqlCommand command = new SqlCommand(query, db);
            command.Parameters.Add(new SqlParameter("stackName", stackName));

            db.Open();
            result = command.ExecuteNonQuery() > 0;
            db.Close();

            return result;
        }

        public static List<Stack> GetStacks()
        {
            List<Stack> result = new();

            string query = $@"SELECT StackId, StackName FROM Stack";

            SqlCommand command = new SqlCommand(query, db);

            db.Open();

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Stack
                {
                    StackId = reader.GetInt32(0),
                    StackName = reader.GetString(1)
                });
            }

            db.Close();

            return result;
        }

        public static bool UpdateStack(string oldName, string newName)
        {
            bool result = false;

            string query = $@"IF EXISTS (SELECT TOP 1 1 FROM Stack WHERE StackName = @newName)
                              BEGIN
                                  RETURN;
                              END

                              UPDATE Stack SET StackName = @newName WHERE StackName = @oldName;";

            SqlCommand command = new SqlCommand(query, db);
            command.Parameters.Add(new SqlParameter("oldName", oldName));
            command.Parameters.Add(new SqlParameter("newName", newName));

            db.Open();
            result = command.ExecuteNonQuery() > 0;
            db.Close();

            return result;
        }

        public static bool DeleteStack(string stackName)
        {
            bool result = false;

            string query = $@"DELETE FROM Stack WHERE StackName = @stackName";

            SqlCommand command = new SqlCommand(query, db);
            command.Parameters.Add(new SqlParameter("stackName", stackName));

            db.Open();
            result = command.ExecuteNonQuery() > 0;
            db.Close();

            return result;
        }
        #endregion

        #region Card
        public static int InsertCard(string cardStackName, string cardQuestion, string cardAnswer)
        {
            int result = 0;

            string query = $@"DECLARE @cardStackId INT;

                              SELECT @cardStackId = StackId
                              FROM Stack 
                              WHERE StackName = @cardStackName;
                              
                              IF (@cardStackId IS NULL)
                              BEGIN
                                  SET @result = -1;
                                  RETURN;
                              END

                              IF EXISTS (SELECT TOP 1 1 FROM Card WHERE CardQuestion = @cardQuestion)
                              BEGIN
                                  SET @result = -2;
                                  RETURN;
                              END

                              INSERT INTO Card (CardStackId, CardQuestion, CardAnswer) VALUES (@cardStackId, @cardQuestion, @cardAnswer);
                              SET @result = 1;
                              RETURN;";

            var outputParameter = new SqlParameter("result", SqlDbType.Int);
            outputParameter.Direction = ParameterDirection.Output;

            SqlCommand command = new SqlCommand(query, db);
            command.Parameters.Add(new SqlParameter("cardStackName", cardStackName));
            command.Parameters.Add(new SqlParameter("cardQuestion", cardQuestion));
            command.Parameters.Add(new SqlParameter("cardAnswer", cardAnswer));
            command.Parameters.Add(outputParameter);

            db.Open();
            command.ExecuteNonQuery();
            result = (int)outputParameter.Value;
            db.Close();

            return result;
        }

        public static List<Card> GetCards(string cardStackName)
        {
            List<Card> result = new();

            string query = $@"DECLARE @cardStackId INT;

                              SELECT @cardStackId = StackId
                              FROM Stack
                              Where StackName = @cardStackName;

                              SELECT CardId, CardStackId, CardQuestion, CardAnswer FROM Card Where CardStackId = @cardStackId";

            SqlCommand command = new SqlCommand(query, db);
            command.Parameters.Add(new SqlParameter("cardStackName", cardStackName));

            db.Open();

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Card
                {
                    CardId = reader.GetInt32(0),
                    CardStackId = reader.GetInt32(1),
                    CardQuestion = reader.GetString(2),
                    CardAnswer = reader.GetString(3)
                });
            }

            db.Close();

            return result;
        }

        public static bool UpdateCard(int cardId, string cardStackName, string cardQuestion, string cardAnswer) {
            bool result = false;

            string query = $@"DECLARE @cardStackId INT;

                              SELECT @cardStackId = StackId
                              FROM Stack 
                              WHERE StackName = @cardStackName;

                              IF EXISTS (SELECT TOP 1 1 FROM Card WHERE CardId != @cardId AND CardStackId = @cardStackId AND CardQuestion = @cardQuestion)
                              BEGIN
                                  RETURN;
                              END
    
                              UPDATE Card SET 
                                  CardQuestion = @cardQuestion
                                , CardAnswer   = @cardAnswer
                              WHERE CardId = @cardId";

            SqlCommand command = new SqlCommand(query, db);
            command.Parameters.Add(new SqlParameter("cardId", cardId));
            command.Parameters.Add(new SqlParameter("cardStackName", cardStackName));
            command.Parameters.Add(new SqlParameter("cardQuestion", cardQuestion));
            command.Parameters.Add(new SqlParameter("cardAnswer", cardAnswer));

            db.Open();
            result = command.ExecuteNonQuery() > 0;
            db.Close();

            return result;
        }

        public static bool DeleteCard(int cardId)
        {
            bool result = false;

            string query = $@"DELETE FROM Card WHERE CardId = @cardId";

            SqlCommand command = new SqlCommand(query, db);
            command.Parameters.Add(new SqlParameter("cardId", cardId));

            db.Open();
            result = command.ExecuteNonQuery() > 0;
            db.Close();

            return result;
        }
        #endregion

        #region StudySession
        public static void InsertStudySession(StudySession studySession, string stackName) {
            string query = $@"DECLARE @studySessionStackId INT;

                              SELECT @studySessionStackId = StackId
                              FROM Stack
                              Where StackName = @studySessionStackName;

                              INSERT INTO StudySession (StudySessionStackId, StudySessionDate, StudySessionPoints, StudySessionMaxPoints) 
                              VALUES (@studySessionStackId, @studySessionDate, @studySessionPoints, @studySessionMaxPoints);";                 
                                                        
            SqlCommand command = new SqlCommand(query, db);
            command.Parameters.Add(new SqlParameter("studySessionStackName", stackName));
            command.Parameters.Add(new SqlParameter("studySessionDate", studySession.StudySessionDate));
            command.Parameters.Add(new SqlParameter("studySessionPoints", studySession.StudySessionPoints));
            command.Parameters.Add(new SqlParameter("studySessionMaxPoints", studySession.StudySessionMaxPoints));

            db.Open();
            command.ExecuteNonQuery();
            db.Close();
        }

        public static List<StudySession> GetStudySessions(string stackName) {
            List<StudySession> result = new();

            string query = $@"DECLARE @studySessionStackId INT;

                              SELECT @studySessionStackId = StackId
                              FROM Stack
                              Where StackName = @studySessionStackName;

                              SELECT StudySessionId, StudySessionStackId, StudySessionDate, StudySessionPoints, StudySessionMaxPoints FROM StudySession Where StudySessionStackId = @studySessionStackId";

            SqlCommand command = new SqlCommand(query, db);
            command.Parameters.Add(new SqlParameter("studySessionStackName", stackName));

            db.Open();

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new StudySession
                {
                    StudySessionId = reader.GetInt32(0),
                    StudySessionStackId = reader.GetInt32(1),
                    StudySessionDate = reader.GetDateTime(2),
                    StudySessionPoints = reader.GetInt32(3),
                    StudySessionMaxPoints = reader.GetInt32(4)
                });
            }

            db.Close();

            return result;
        }
        #endregion
    }
}
