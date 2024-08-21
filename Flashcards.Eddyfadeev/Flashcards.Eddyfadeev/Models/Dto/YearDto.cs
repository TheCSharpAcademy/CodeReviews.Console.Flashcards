using Flashcards.Eddyfadeev.Interfaces.Models;

namespace Flashcards.Eddyfadeev.Models.Dto;

/// <summary>
/// Represents a data transfer object for a year.
/// </summary>
internal record YearDto : IYear
{
    public int ChosenYear { get; set; }
}