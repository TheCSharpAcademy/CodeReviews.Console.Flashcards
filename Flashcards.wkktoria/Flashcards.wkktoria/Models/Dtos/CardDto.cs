namespace Flashcards.wkktoria.Models.Dtos;

internal class CardDto
{
    public int DtoId { get; set; }
    public string Front { get; init; } = string.Empty;
    public string Back { get; init; } = string.Empty;
}