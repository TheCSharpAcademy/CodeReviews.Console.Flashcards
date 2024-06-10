using Dapper;
using Flashcards.kalsson.Models;
using Microsoft.Data.SqlClient;
using System;

namespace Flashcards.kalsson.Data
{
    public class StackRepository
    {
        private readonly string _connectionString;

        public StackRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Retrieves all stacks from the database.
        /// </summary>
        /// <returns>An IEnumerable collection of Stack objects.</returns>
        public IEnumerable<Stack> GetAllStacks()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                return connection.Query<Stack>("SELECT * FROM Stacks");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to retrieve all stacks.", ex);
            }
        }

        /// <summary>
        /// Retrieves a stack by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the stack to retrieve.</param>
        /// <returns>The stack with the specified ID, or null if the stack does not exist.</returns>
        public Stack GetStackById(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                return connection.QuerySingleOrDefault<Stack>("SELECT * FROM Stacks WHERE Id = @Id", new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to retrieve the stack by ID.", ex);
            }
        }

        /// <summary>
        /// Adds a new stack to the database.
        /// </summary>
        /// <param name="stack">The Stack object representing the stack to be added.</param>
        /// <exception cref="Exception">Throws an exception if an error occurs while trying to add the stack.</exception>
        public void AddStack(Stack stack)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Execute("INSERT INTO Stacks (Name) VALUES (@Name)", stack);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to add the stack.", ex);
            }
        }

        /// <summary>
        /// Update the specified stack.
        /// </summary>
        /// <param name="stack">The stack to be updated.</param>
        /// <exception cref="Exception">Thrown when an error occurs while trying to update the stack.</exception>
        public void UpdateStack(Stack stack)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Execute("UPDATE Stacks SET Name = @Name WHERE Id = @Id", stack);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to update the stack.", ex);
            }
        }

        /// <summary>
        /// Deletes a stack with the specified ID from the database.
        /// </summary>
        /// <param name="id">The ID of the stack to be deleted.</param>
        public void DeleteStack(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Execute("DELETE FROM Stacks WHERE Id = @Id", new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to delete the stack.", ex);
            }
        }
    }
}