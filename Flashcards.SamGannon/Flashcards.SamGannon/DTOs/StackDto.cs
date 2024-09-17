using DataAccess.Models;

namespace Flashcards.SamGannon.DTOs
{
    public class StackDto
    {
        public string? StackName { get; set; } = string.Empty;

        public StackDto() { }

        public StackDto(StackModel stack)
        {
            StackName = stack.StackName;
        }

        public static List<StackDto> ToDto(List<StackModel> stacks)
        {
            List<StackDto> returnStack = new List<StackDto>();

            foreach (var stack in stacks)
            {
                var stackDto = new StackDto(stack);
                returnStack.Add(stackDto);
            }
            
            return returnStack;
        }
    }
}
