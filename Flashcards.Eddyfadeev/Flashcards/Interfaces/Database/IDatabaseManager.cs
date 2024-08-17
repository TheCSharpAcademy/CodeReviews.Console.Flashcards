using Flashcards.Models.Entity;

namespace Flashcards.Interfaces.Database;

/// <summary>
/// Represents a database manager that provides CRUD operations and database initialization.
/// </summary>
internal interface IDatabaseManager
{
    /// <summary>
    /// Inserts a new entity into the database.
    /// </summary>
    /// <param name="query">The query to execute for inserting the entity.</param>
    /// <param name="parameters">The parameters required by the query.</param>
    /// <returns>The number of affected rows.</returns>
    internal int InsertEntity(string query, object parameters);

    /// <summary>
    /// Retrieves all entities of type TEntity from the database.
    /// </summary>
    /// <typeparam name="TEntity">The type of entities to be retrieved.</typeparam>
    /// <param name="query">The SQL query to execute.</param>
    /// <returns>A collection of entities retrieved from the database.</returns>
    internal IEnumerable<TEntity> GetAllEntities<TEntity>(string query);

    /// <summary>
    /// Retrieves all entities of type TEntity from the database.
    /// </summary>
    /// <typeparam name="TEntity">The type of entities to retrieve.</typeparam>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <returns>A collection of entities.</returns>
    internal IEnumerable<TEntity> GetAllEntities<TEntity>(string query, object parameters);

    /// <summary>
    /// Deletes an entry from the database based on the given query and parameters.
    /// </summary>
    /// <param name="query">The SQL query used for deleting the entry.</param>
    /// <param name="parameters">The parameters to be used in the query.</param>
    /// <returns>The number of rows affected by the delete operation.</returns>
    internal int DeleteEntry(string query, object parameters);

    /// <summary>
    /// Updates an entry in the database.
    /// </summary>
    /// <param name="query">The SQL query to update the entry.</param>
    /// <param name="parameters">The parameters required for the query.</param>
    /// <returns>The number of affected rows.</returns>
    internal int UpdateEntry(string query, object parameters);
    
    /// <summary>
    /// Bulk inserts multiple records into the database.
    /// </summary>
    /// <param name="stacks">The list of Stack objects to insert.</param>
    /// <param name="flashcards">The list of Flashcard objects to insert.</param>
    /// <returns>Returns true if the records were successfully inserted, false otherwise.</returns>
    internal bool BulkInsertRecords(List<Stack> stacks, List<Flashcard> flashcards);

    /// <summary>
    /// Drops all foreign key constraints in the database.
    /// </summary>
    /// <remarks>
    /// This method executes a query to retrieve all foreign key constraints in the database.
    /// Then, it iterates over the result and executes a query to drop each constraint using ALTER TABLE statement.
    /// If any error occurs during the process, an exception is thrown and a message is displayed to the console.
    /// </remarks>
    /// <exception cref="Exception">Thrown if there was a problem dropping the foreign key constraints.</exception>
    internal void DropForeignKeyConstraints();

    /// <summary>
    /// Deletes all tables from the database.
    /// </summary>
    internal void DeleteTables();
}