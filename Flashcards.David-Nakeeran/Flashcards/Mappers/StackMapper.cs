
using Flashcards.Models;

namespace Flashcards.Mappers;

class StackMapper
{
    internal StackDTO MapStackToDTO(Stack stack, int id)
    {
        return new StackDTO
        {
            DisplayId = id,
            Name = stack.StackName
        };
    }
}