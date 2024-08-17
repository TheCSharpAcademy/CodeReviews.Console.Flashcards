namespace Flashcards.Eddyfadeev.Interfaces.Database;

/// <summary>
/// The IDatabaseInitializer interface is used to initialize the database.
/// </summary>
internal interface IDatabaseInitializer
{
    /// <summary>
    /// Initializes the database by creating necessary tables and inserting seed data.
    /// </summary>
    internal void Initialize();
}