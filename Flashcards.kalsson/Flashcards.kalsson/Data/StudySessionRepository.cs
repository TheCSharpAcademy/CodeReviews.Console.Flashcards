using Dapper;
using Flashcards.kalsson.Models;
using Microsoft.Data.SqlClient;
using System;

namespace Flashcards.kalsson.Data
{
    public class StudySessionRepository
    {
        private readonly string _connectionString;

        public StudySessionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Retrieves all study sessions.
        /// </summary>
        /// <returns>An IEnumerable collection of StudySession objects.</returns>
        public IEnumerable<StudySession> GetAllStudySessions()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                return connection.Query<StudySession>("SELECT * FROM StudySessions");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to retrieve all study sessions.", ex);
            }
        }

        /// <summary>
        /// Retrieves a study session by its ID.
        /// </summary>
        /// <param name="id">The ID of the study session to retrieve.</param>
        /// <returns>The study session with the specified ID.</returns>
        public StudySession GetStudySessionById(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                return connection.QuerySingleOrDefault<StudySession>("SELECT * FROM StudySessions WHERE Id = @Id",
                    new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to retrieve the study session by ID.", ex);
            }
        }

        /// <summary>
        /// Adds a study session to the database.
        /// </summary>
        /// <param name="studySession">The study session to add.</param>
        /// <exception cref="Exception">Thrown if an error occurs while trying to add the study session.</exception>
        public void AddStudySession(StudySession studySession)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Execute(
                    "INSERT INTO StudySessions (StackId, StudyDate, Score) VALUES (@StackId, @StudyDate, @Score)",
                    studySession);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to add the study session.", ex);
            }
        }
    }
}