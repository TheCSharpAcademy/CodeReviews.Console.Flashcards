namespace Flashcards.StevieTV.Models;

internal class Stack
{
    public int StackId { get; set; }
    public string Name { get; set; } = string.Empty;
}

internal class StackDTO
{
    public string Name { get; set; } = string.Empty;
}

internal static class StackMapper
{
    internal static StackDTO StackMapToDTO(Stack stack)
    {
        return new StackDTO
        {
            Name = stack.Name
        };
    }
}