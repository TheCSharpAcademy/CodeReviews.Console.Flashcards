using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Extensions;

namespace Flashcards.Eddyfadeev.Models.Entity;

/// <summary>
/// Represents a flashcard used for studying.
/// </summary>
internal class Flashcard : IFlashcard, IDbEntity<IFlashcard>
{
    public int Id { get; set; }
    public int? StackId { get; set; }
    public string? Question { get; set; }
    public string? Answer { get; set; }

    /// <summary>
    /// Maps an instance of <see cref="IFlashcard"/> to an instance of <see cref="FlashcardDto"/>.
    /// </summary>
    /// <param name="flashcard">The <see cref="IFlashcard"/> instance to map.</param>
    /// <returns>An instance of <see cref="FlashcardDto"/> representing the mapped <see cref="IFlashcard"/>.</returns>
    public IFlashcard MapToDto() => this.ToDto();
    
    public override string ToString() => $"Question: {Question}, Answer: {Answer}";
}