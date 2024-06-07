namespace Flashcards.Models;

internal class StackDto
{
    public string StackName { get; set; } = default!;
}

internal class StackUpdateDto : Stack
{
    public int Id { get; set; }
}