using Flashcards.ukpagrace.Entity;
using Flashcards.ukpagrace.DTO;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Flashcards.ukpagrace.Database
{
    class StudySessionDatabase
    {
        string connectionString = ConfigurationManager.AppSettings["ConfigurationString"] ?? string.Empty;
        public void Create()
        {
            try
            {
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"
                    IF NOT EXISTS(
                        SELECT * 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'studySession' AND TABLE_SCHEMA = 'dbo'
                    )
                    BEGIN
                        CREATE TABLE dbo.studySession(
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            StackId INT NOT NULL, 
                            StartDate DATETIME NOT NULL,
                            EndDate DATETIME NOT NULL,
                            Score INT NOT NULL,
                            FOREIGN KEY(StackId)
                            REFERENCES dbo.stack(Id)
                            ON DELETE CASCADE ON UPDATE NO ACTION
                        );
                    END
                ";
                sqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Insert(int stackId, DateTime startDate, DateTime endDate, int score)
        {
            try
            {
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"INSERT INTO studySession VALUES (@StackId, @StartDate, @EndDate, @Score)";
                cmd.Parameters.AddWithValue("@StackId", stackId);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.Parameters.AddWithValue("@Score", score);
                sqlConnection.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("inserted 1 record into stack");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public bool CheckAnswer(int stackId, string answer, string question)
        {
            try
            {
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"SELECT Id FROM flashcard 
                    WHERE StackId=@StackId 
                    AND Question=@Question
                    AND Answer=@Answer"
                ;
                cmd.Parameters.AddWithValue("@StackId", stackId);
                cmd.Parameters.AddWithValue("@Question", question);
                cmd.Parameters.AddWithValue("@Answer", answer);
                sqlConnection.Open();
                SqlDataReader table = cmd.ExecuteReader();
                return table.HasRows;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<StudySessionEntity> Get()
        {
            try
            {
                List<StudySessionEntity> records = new List<StudySessionEntity>();
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM studySession";
                sqlConnection.Open();
                SqlDataReader table = cmd.ExecuteReader();
                int count = 1;
                if (table.HasRows)
                {

                    while (table.Read())
                    {
                        records.Add(new StudySessionEntity()
                        {
                            Id = count++,
                            StartDate = table.GetDateTime(2),
                            EndDate = table.GetDateTime(3),
                            Score = table.GetInt32(4)
                        });
                    }
                }
                return records;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<NumberOfSessionPerMonthDTO> NumberofSessionPerMonth()
        {
            try
            {
                List<NumberOfSessionPerMonthDTO> records = new();
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "SELECT count(id), MONTH(EndDate) FROM studySession group by MONTH(EndDate);";
                sqlConnection.Open();
                SqlDataReader table = cmd.ExecuteReader();
                if (table.HasRows)
                {
                    while (table.Read())
                    {
                        records.Add(new NumberOfSessionPerMonthDTO()
                        {
                            Count = table.GetInt32(0),
                            Month = table.GetInt32(1)
                        });
                    }
                }
                return records;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }


        public List<AverageScorePerMonthDTO> AverageScorePerMonth()
        {
            try
            {
                List<AverageScorePerMonthDTO> records = new();
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"
                    SELECT 
                        SUM(score) / COUNT(id) AS AverageScore,
                        MONTH(EndDate) AS Month
                        
                    FROM 
                        studySession
                    GROUP BY 
                        MONTH(EndDate);
                ";
                sqlConnection.Open();
                SqlDataReader table = cmd.ExecuteReader();
                if (table.HasRows)
                {
                    while (table.Read())
                    {
                        records.Add(new AverageScorePerMonthDTO()
                        {
                            AverageScore = table.GetInt32(0),
                            Month = table.GetInt32(1)
                        });
                    }
                }
                return records;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}