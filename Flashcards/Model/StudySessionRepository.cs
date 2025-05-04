﻿using Microsoft.Data.SqlClient;

namespace Flashcards.Model
{
    internal class StudySessionRepository
    {
        private readonly string _connectionString;

        public StudySessionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SaveStudySessionStats(StudySession session)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string query = @"INSERT INTO StudySessionStats (StackId, SessionStartTime, PercentageCorrect) VALUES (@StackID, @SessionStartTime, @PercentageCorrect)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@StackID", session.StackId));
                    command.Parameters.Add(new SqlParameter("@SessionStartTime", session.SessionStartTime));
                    command.Parameters.Add(new SqlParameter("@PercentageCorrect", session.PercentageCorrect));
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<StudySession> GetAllStudySessions()
        {
            var sessions = new List<StudySession>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT ss.SessionID, ss.StackID, ss.SessionStartTime, ss.PercentageCorrect, s.StackName 
                    FROM StudySessionStats ss 
                    JOIN Stacks s ON ss.StackID = s.StackID
                    ORDER BY SessionStartTime DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var session = new StudySession
                            {
                                Id = reader.GetInt32(0),
                                StackId = reader.GetInt32(1),
                                SessionStartTime = reader.GetDateTime(2),
                                PercentageCorrect = reader.GetDecimal(3),
                                StackName = reader.GetString(4)
                            };
                            sessions.Add(session);
                        }
                    }
                }
            }

            return sessions;
        }

        public void CreateTable()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudySessionStats')
                    BEGIN
                        CREATE TABLE StudySessionStats (
                        SessionID INT IDENTITY(1,1) PRIMARY KEY,
                        StackID INT NOT NULL,
                        SessionStartTime DATETIME NOT NULL,
                        PercentageCorrect DECIMAL(5,2) NOT NULL,
                        FOREIGN KEY (StackID) REFERENCES Stacks(StackId) ON DELETE CASCADE
                        );
                    END;";

                using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"An error occurred while creating the StudySessionStats table: {ex.Message}");
                    }
                }
            }
        }

        public void SeedStudySessionStats()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string insertStudySessionStatsQuery = @"
                    INSERT INTO StudySessionStats (StackID, SessionStartTime, PercentageCorrect)
                      VALUES (@StackID, @SessionStartTime, @PercentageCorrect);";

                var studySessions = new List<(int StackID, DateTime SessionStartTime, decimal PercentageCorrect)>
                {
                    (1, DateTime.Now, 75.00m),
                    (2, DateTime.Now.AddMinutes(+30), 100.00m),
                    (3, DateTime.Now.AddMinutes(+60), 25.00m)
                };

                foreach (var (StackID, SessionStartTime, PercentageCorrect) in studySessions)
                {
                    using (SqlCommand command = new SqlCommand(insertStudySessionStatsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@StackID", StackID);
                        command.Parameters.AddWithValue("@SessionStartTime", SessionStartTime);
                        command.Parameters.AddWithValue("@PercentageCorrect", PercentageCorrect);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
