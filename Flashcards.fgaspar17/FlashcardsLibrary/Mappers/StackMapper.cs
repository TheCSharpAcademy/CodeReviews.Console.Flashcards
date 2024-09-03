namespace FlashcardsLibrary;
public static class StackMapper
{
    public static StackDTO MapToDTO(Stack stack)
    {
        return new StackDTO
        {
            Name = stack.Name,
        };
    }
}