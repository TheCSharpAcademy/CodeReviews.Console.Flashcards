using Dapper;
using Flashcards.kalsson.Models;
using Microsoft.Data.SqlClient;
using System;

namespace Flashcards.kalsson.Data
{
    public class FlashcardRepository
    {
        private readonly string _connectionString;

        public FlashcardRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Retrieves all flashcards for a given stack ID.
        /// </summary>
        /// <param name="stackId">The ID of the stack.</param>
        /// <returns>A collection of Flashcard objects.</returns>
        public IEnumerable<Flashcard> GetAllFlashcards(int stackId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                return connection.Query<Flashcard>("SELECT * FROM Flashcards WHERE StackId = @StackId",
                    new { StackId = stackId });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to retrieve all flashcards.", ex);
            }
        }

        /// <summary>
        /// Retrieves a flashcard by its ID.
        /// </summary>
        /// <param name="id">The ID of the flashcard to retrieve.</param>
        /// <returns>The flashcard with the specified ID.</returns>
        public Flashcard GetFlashcardById(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                return connection.QuerySingleOrDefault<Flashcard>("SELECT * FROM Flashcards WHERE Id = @Id",
                    new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to retrieve the flashcard by ID.", ex);
            }
        }

        /// <summary>
        /// Adds a new flashcard to the database.
        /// </summary>
        /// <param name="flashcard">The flashcard to be added.</param>
        /// <exception cref="Exception">Thrown when an error occurs while trying to add the flashcard.</exception>
        public void AddFlashcard(Flashcard flashcard)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Execute(
                    "INSERT INTO Flashcards (StackId, Question, Answer) VALUES (@StackId, @Question, @Answer)",
                    flashcard);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to add the flashcard.", ex);
            }
        }

        /// <summary>
        /// Updates the specified flashcard.
        /// </summary>
        /// <param name="flashcard">The flashcard object to be updated.</param>
        /// <exception cref="Exception">Thrown if an error occurs while trying to update the flashcard.</exception>
        public void UpdateFlashcard(Flashcard flashcard)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Execute("UPDATE Flashcards SET Question = @Question, Answer = @Answer WHERE Id = @Id",
                    flashcard);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to update the flashcard.", ex);
            }
        }

        /// <summary>
        /// Deletes a flashcard from the database.
        /// </summary>
        /// <param name="id">The ID of the flashcard to delete.</param>
        /// <exception cref="Exception">Thrown if an error occurs while trying to delete the flashcard.</exception>
        public void DeleteFlashcard(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Execute("DELETE FROM Flashcards WHERE Id = @Id", new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to delete the flashcard.", ex);
            }
        }
    }
}