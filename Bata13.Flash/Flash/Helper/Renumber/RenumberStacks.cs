using Flash.Helper.DTO;

namespace Flash.Helper.Renumber;
internal class RenumberStacks
{
    internal static void GetRenumberStacks(List<StacksDto> stacks)
    {
        for (int i = 0; i < stacks.Count; i++)
        {
            stacks[i].Stack_Primary_Id = i + 1;
        }
    }
}
