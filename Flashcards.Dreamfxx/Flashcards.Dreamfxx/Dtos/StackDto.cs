using Flashcards.Dreamfxx.Models;
namespace Flashcards.Dreamfxx.Dtos;

public class StackDto
{
    public required string StackName { get; set; }
    public List<Flashcard>? Flashcards { get; set; }
}
