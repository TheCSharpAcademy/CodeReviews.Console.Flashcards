namespace Flashcards;

class StacksDTO(Stacks stack, int stackID)
{
    public int StackID = stackID;
    public string StackName = stack.StackName;
}