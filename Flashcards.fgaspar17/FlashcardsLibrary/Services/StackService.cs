namespace FlashcardsLibrary;
public static class StackService
{
    public static List<StackDTO> GetStacks()
    {
        List<Stack> stacks = StackController.GetStacks();
        return stacks.Select(stack => StackMapper.MapToDTO(stack)).ToList();
    }
}