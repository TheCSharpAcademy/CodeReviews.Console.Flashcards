using Flashcards.Eddyfadeev.Extensions;
using Flashcards.Eddyfadeev.Interfaces.Models;

namespace Flashcards.Eddyfadeev.Models.Entity;

/// Represents a year, obviously.
internal class Year : IYear, IDbEntity<IYear>
{
    public int ChosenYear { get; set; }

    /// <summary>
    /// Maps an object that implements the <see cref="IYear"/> interface to a YearDto object.
    /// </summary>
    /// <param name="year">The year object to convert.</param>
    /// <returns>A YearDto object converted from the IYear object.</returns>
    public IYear MapToDto() => this.ToDto();

    public override string ToString() => ChosenYear.ToString();
}