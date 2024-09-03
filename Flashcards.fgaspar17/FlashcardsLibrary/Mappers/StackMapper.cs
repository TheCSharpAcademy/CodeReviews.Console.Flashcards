namespace FlashcardsLibrary;
public static class StackMapper
{
    public static StackDto MapToDto(Stack stack)
    {
        return new StackDto
        {
            Name = stack.Name,
        };
    }
}