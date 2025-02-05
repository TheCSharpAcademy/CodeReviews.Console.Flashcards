using Dapper;
using cacheMe512.Flashcards.Models;
using cacheMe512.Flashcards.DTOs;

namespace cacheMe512.Flashcards.Controllers
{
    internal class StudySessionController
    {
        private readonly StackController _stackController = new();

        public IEnumerable<StudySessionDto> GetAllSessions()
        {
            try
            {
                using var connection = Database.GetConnection();

                var sessions = connection.Query(
                    @"SELECT ss.Id, ss.StackId, ss.Date, ss.TotalQuestions, ss.Score, s.Name AS StackName
              FROM study_sessions ss
              JOIN stacks s ON ss.StackId = s.Id
              ORDER BY ss.Date DESC, s.Name ASC, ss.Score DESC"
                ).Select(row => new StudySessionDto(
                    row.Id,
                    row.StackName,
                    row.Date,
                    row.TotalQuestions,
                    row.Score
                )).ToList();

                return sessions;
            }
            catch (Exception ex)
            {
                Utilities.DisplayMessage($"Error retrieving study sessions: {ex.Message}", "red");
                return Enumerable.Empty<StudySessionDto>();
            }
        }



        public IEnumerable<StudySessionDto> GetActiveSessions()
        {
            try
            {
                using var connection = Database.GetConnection();
                using var transaction = connection.BeginTransaction();

                connection.Execute(
                    @"WITH CTE AS (
                        SELECT Id, ROW_NUMBER() OVER (ORDER BY Position) AS NewPosition
                        FROM study_sessions
                        WHERE Active = 1
                    )
                    UPDATE study_sessions
                    SET Position = CTE.NewPosition
                    FROM CTE
                    WHERE study_sessions.Id = CTE.Id;",
                    transaction: transaction
                );
                transaction.Commit();

                var sessions = connection.Query<StudySession>(
                    "SELECT Id, StackId, Date, TotalQuestions, Score FROM study_sessions WHERE Active = 1 ORDER BY Position"
                ).ToList();

                return sessions.Select(session =>
                {
                    var stack = _stackController.GetAllStacks().FirstOrDefault(s => s.Id == session.StackId);
                    return new StudySessionDto(session.Id, stack?.Name ?? "Unknown Stack", session.Date, session.TotalQuestions, session.Score);
                });
            }
            catch (Exception ex)
            {
                Utilities.DisplayMessage($"Error retrieving active sessions: {ex.Message}", "red");
                return Enumerable.Empty<StudySessionDto>();
            }
        }

        public StudySessionDto GetSessionById(int sessionId)
        {
            try
            {
                using var connection = Database.GetConnection();
                var session = connection.QueryFirstOrDefault<StudySession>(
                    "SELECT Id, StackId, Date, TotalQuestions, Score FROM study_sessions WHERE Id = @Id",
                    new { Id = sessionId }
                );

                if (session == null)
                    return null;

                var stack = _stackController.GetAllStacks().FirstOrDefault(s => s.Id == session.StackId);
                return new StudySessionDto(0, stack?.Name ?? "Unknown Stack", session.Date, session.TotalQuestions, session.Score);
            }
            catch (Exception ex)
            {
                Utilities.DisplayMessage($"Error retrieving session data: {ex.Message}", "red");
                return null;
            }
        }

        public int InsertSession(StudySession session)
        {
            try
            {
                using var connection = Database.GetConnection();
                using var transaction = connection.BeginTransaction();

                int nextPosition = connection.ExecuteScalar<int>(
                    "SELECT COALESCE(MAX(Position), 0) + 1 FROM study_sessions",
                    transaction: transaction
                );

                var sessionId = connection.ExecuteScalar<int>(
                    "INSERT INTO study_sessions (StackId, Date, Score, TotalQuestions, Active, Position) " +
                    "OUTPUT INSERTED.Id VALUES (@StackId, @Date, @Score, @TotalQuestions, 1, @Position);",
                    new { session.StackId, session.Date, session.Score, session.TotalQuestions, Position = nextPosition },
                    transaction: transaction
                );

                transaction.Commit();
                return sessionId;
            }
            catch (Exception ex)
            {
                Utilities.DisplayMessage($"Error inserting study session: {ex.Message}", "red");
                return -1;
            }
        }

        public void EndSession(int sessionId)
        {
            try
            {
                using var connection = Database.GetConnection();
                using var transaction = connection.BeginTransaction();

                int rowsAffected = connection.Execute(
                    "UPDATE study_sessions SET Active = 0 WHERE Id = @SessionId",
                    new { SessionId = sessionId },
                    transaction: transaction
                );

                if (rowsAffected == 0)
                {
                    Console.WriteLine($"[red]Error: No session found with ID {sessionId} or session was already ended.[/]");
                    return;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Utilities.DisplayMessage($"Error ending study session: {ex.Message}", "red");
            }
        }


        public void UpdateSessionScore(int sessionId, int newCorrectAnswers, int newTotalQuestions)
        {
            try
            {
                using var connection = Database.GetConnection();
                using var transaction = connection.BeginTransaction();

                var existingSession = connection.QueryFirstOrDefault<dynamic>(
                    "SELECT Score, TotalQuestions FROM study_sessions WHERE Id = @Id",
                    new { Id = sessionId },
                    transaction: transaction
                );

                if (existingSession == null)
                {
                    Utilities.DisplayMessage($"Session with ID {sessionId} not found.", "red");
                    return;
                }

                int updatedScore = existingSession.Score + newCorrectAnswers;
                int updatedTotalQuestions = existingSession.TotalQuestions + newTotalQuestions;

                connection.Execute(
                    "UPDATE study_sessions SET Score = @Score, TotalQuestions = @TotalQuestions WHERE Id = @Id",
                    new { Score = updatedScore, TotalQuestions = updatedTotalQuestions, Id = sessionId },
                    transaction: transaction
                );

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Utilities.DisplayMessage($"Error updating study session score: {ex.Message}", "red");
            }
        }
    }
}
