namespace Flashcards.ConsoleApp.Models;

/// <summary>
/// Represents a choice which a user can select.
/// </summary>
internal class UserChoice
{
    #region Constructors

    public UserChoice(int id, string name)
    {
        Id = id;
        Name = name;
    }

    #endregion
    #region Properties

    internal int Id { get; init; }

    internal string? Name { get; init; }

    #endregion
}
