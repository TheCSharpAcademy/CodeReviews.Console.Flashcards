using FlashCards.Dtos;
using FlashCards.Models;

namespace FlashCards.Mappers;

internal static class StackMapper
{
    public static StackDto ToStackDto(this Stack stack)
    {
        return new  StackDto
        {
            name = stack.name
        };
    }
}
