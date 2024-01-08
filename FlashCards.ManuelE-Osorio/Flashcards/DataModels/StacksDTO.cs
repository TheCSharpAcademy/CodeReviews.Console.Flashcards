namespace Flashcards;

class StacksDto(Stacks stack, int stackID)
{
    public int StackID = stackID;
    public string StackName = stack.StackName;
}