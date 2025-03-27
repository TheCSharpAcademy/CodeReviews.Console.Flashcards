using Dapper;
using Microsoft.Data.SqlClient; //for sql server connection
using FlashcardsApp.Models;
using FlashcardsApp.DTOs;
using Spectre.Console;
using FlashcardsApp.UI.Helpers;

namespace FlashcardsApp.Services
{
    internal class DatabaseService
    {
        private readonly string _connectionString;

        internal DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void TestConnection()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
            }
        }

        internal void DeleteFlashcard(int stackId, int flashcardId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "DELETE FROM Flashcards WHERE StackId = @StackId AND FlashcardId = @FlashcardId";

                var rowsAffected = connection.Execute(sql, new { StackId = stackId, FlashcardId = flashcardId });

                if (rowsAffected > 0)
                {
                    Console.WriteLine("\nDeletion successful.");
                    Console.WriteLine($"\n{rowsAffected} row(s) deleted.");
                }
                else
                {
                    Console.WriteLine("\nDeletion has failed");
                }
            }
        }

        internal void DeleteStack(int stackId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "DELETE FROM Stacks WHERE StackId = @StackId";

                var rowsAffected = connection.Execute(sql, new { StackId = stackId });

                if (rowsAffected > 0)
                {
                    Console.WriteLine("\nDeletion successful.");
                    Console.WriteLine($"\n{rowsAffected} row(s) deleted.");
                }
                else
                {
                    Console.WriteLine("\nDeletion has failed");
                }
            }
        }


        internal void GetAllStacks()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "SELECT * FROM Stacks";

                var stacks = connection.Query<Stack>(sql).ToList();

                TableVisualization.ShowStacksTable(stacks);
            }
        }

        internal List<Stack> GetAllStacksAsList()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "SELECT * FROM Stacks";

                var stacks = connection.Query<Stack>(sql).ToList();

                return stacks;
            }
        }

        internal Flashcard? GetFlashcardByFlashcardId(int flashcardId, int stackId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "SELECT FlashcardId, StackId, Front, Back, CreatedDate FROM Flashcards WHERE StackId = @StackId AND FlashcardId = @FlashcardId";

                var flashcard = connection.QuerySingleOrDefault<Flashcard>(sql, new { StackId = stackId, FlashcardId = flashcardId });

                if (flashcard == null)
                {
                    Console.WriteLine($"\nNo flashcard found with ID: {flashcardId} in stack {stackId}");
                    return null;
                }

                return flashcard;
            }
        }


        internal List<FlashcardDTO> GetFlashcardsByID(int stackId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"
                    SELECT
                        ROW_NUMBER() OVER (ORDER BY FlashcardId) as DisplayNumber,
                        FlashcardId,
                        Front,
                        Back,
                        CreatedDate
                    FROM Flashcards
                    WHERE StackId = @StackId";

                var flashcards = connection.Query<FlashcardDTO>(sql, new { StackId = stackId}).ToList();

                TableVisualization.ShowFlashcardsTable(flashcards);

                return flashcards;
            }
        }

        internal List<Flashcard> GetFlashcardsByStackID(int stackId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"
                    SELECT
                        FlashcardId,
                        StackId,
                        Front,
                        Back,
                        CreatedDate
                    FROM Flashcards
                    WHERE StackId = @StackId";

                var flashcards = connection.Query<Flashcard>(sql, new { StackId = stackId }).ToList();

                return flashcards;
            }
        }

        internal void GetStudyHistory()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "SELECT * FROM StudySessions";

                var sessions = connection.Query<StudySession>(sql).ToList();

                var sql2 = "SELECT * FROM Stacks";

                var stacks = connection.Query<Stack>(sql2).ToList();

                if (sessions.Count > 0)
                {
                    TableVisualization.ShowStudySessionsTable(sessions, stacks);
                }
                else
                {
                    Console.WriteLine("\nNo sessions to view!");
                }
            }
        }

        internal void Post(Stack stack)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "INSERT INTO Stacks (Name, Description, CreatedDate) VALUES (@Name, @Description, @CreatedDate)";

                var rowsAffected = connection.Execute(sql, new
                {
                    Name = stack.Name,
                    Description = stack.Description,
                    CreatedDate = stack.CreatedDate,
                });

                if (rowsAffected > 0)
                {
                    Console.WriteLine("\nSuccessfully added stack!");
                    Console.WriteLine($"\n{rowsAffected} row(s) inserted.");
                }
                else
                {
                    Console.WriteLine("\nFailed to add stack!");
                }
            }    
        }

        internal void PostFlashcard(Flashcard flashcard)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var checkSql = @"SELECT COUNT(1) 
                        FROM Flashcards 
                        WHERE StackId = @StackId 
                        AND Front = @Front";

                var exists = connection.ExecuteScalar<int>(checkSql, new
                {
                    StackId = flashcard.StackId,
                    Front = flashcard.Front
                }) > 0;

                if (exists)
                {
                    Console.WriteLine("\nA flashcard with this front already exists in this stack!");
                    return;
                }

                var sql = "INSERT INTO Flashcards (StackId, Front, Back, CreatedDate) VALUES (@StackId, @Front, @Back, @CreatedDate)";

                var rowsAffected = connection.Execute(sql, new
                {
                    StackId = flashcard.StackId,
                    Front = flashcard.Front,
                    Back = flashcard.Back,
                    CreatedDate = flashcard.CreatedDate
                });

                if (rowsAffected > 0)
                {
                    Console.WriteLine("\nSuccessfully added flashcard to stack!");
                    Console.WriteLine($"\n{rowsAffected} row(s) inserted.");
                }
                else
                {
                    Console.WriteLine("\nFailed to add flashcard!");
                }
            }
        }

        internal void PostStudySession(StudySession session)
        {
            using (var connection = new SqlConnection( _connectionString))
            {
                connection.Open();

                var sql = "INSERT INTO StudySessions (StackId, Score, StudyDate) VALUES (@StackId, @Score, @StudyDate)";

                var rowsAffected = connection.Execute(sql, new
                {
                    StackId = session.StackId,
                    Score = session.Score,
                    StudyDate = session.StudyDate,
                });

                if (rowsAffected > 0)
                {
                    Console.WriteLine("\nStudy Session was successfully uploaded!");
                    Console.WriteLine($"\n{rowsAffected} row(s) inserted.");
                }
                else
                {
                    Console.WriteLine("\nFailed to upload session!");
                }
            }
        }

        internal void UpdateFlashcard(int stackId, int flashcardId, Flashcard flashcard)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"UPDATE Flashcards SET Front = @Front, Back = @Back, CreatedDate = @CreatedDate WHERE StackId = @StackId AND FlashcardId = @FlashcardId";

                var rowsAffected = connection.Execute(sql, new
                {
                    Front = flashcard.Front,
                    Back = flashcard.Back,
                    CreatedDate = flashcard.CreatedDate,
                    StackId = stackId,
                    FlashcardId = flashcardId
                });
            }
        }
    }
}
