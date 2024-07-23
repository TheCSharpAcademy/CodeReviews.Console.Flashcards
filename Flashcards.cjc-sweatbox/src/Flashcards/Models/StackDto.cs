using Flashcards.Data.Entities;

namespace Flashcards.Models;

/// <summary>
/// Presentation version of the Stack object.
/// </summary>
public class StackDto
{
    #region Constructors

    public StackDto(StackEntity entity)
    {
        Id = entity.Id;
        Name = entity.Name;
    }

    public StackDto(string name)
    {
        Name = name;
    }

    #endregion
    #region Properties

    public int Id { get; init; }

    public string Name { get; init; }

    #endregion
}
