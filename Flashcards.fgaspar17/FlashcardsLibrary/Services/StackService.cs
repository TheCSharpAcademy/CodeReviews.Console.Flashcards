namespace FlashcardsLibrary;
public static class StackService
{
    public static List<StackDto> GetStacks()
    {
        List<Stack> stacks = StackController.GetStacks();
        return stacks.Select(stack => StackMapper.MapToDto(stack)).ToList();
    }
}