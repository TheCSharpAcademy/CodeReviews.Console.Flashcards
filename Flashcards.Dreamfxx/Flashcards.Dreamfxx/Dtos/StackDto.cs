using Flashcards.Dreamfxx.Models;
namespace Flashcards.Dreamfxx.Dtos;

public class StackDto
{
    public string? StackName { get; set; }
    public List<Flashcard>? FlashcardsDto { get; set; }
}
