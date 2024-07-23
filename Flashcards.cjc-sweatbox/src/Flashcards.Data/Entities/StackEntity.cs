using System.Data;
using Flashcards.Data.Extensions;

namespace Flashcards.Data.Entities;

/// <summary>
/// Database version of the Stack object.
/// </summary>
public class StackEntity
{
    #region Constructors

    public StackEntity(IDataReader reader)
    {
        Id = reader.GetInt32("Id");
        Name = reader.GetString("Name");
    }

    #endregion
    #region Properties

    public int Id { get; init; }

    public string Name { get; init; }

    #endregion
}
