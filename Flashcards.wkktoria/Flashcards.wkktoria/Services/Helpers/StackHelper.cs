using Flashcards.wkktoria.Models.Dtos;

namespace Flashcards.wkktoria.Services.Helpers;

internal static class StackHelper
{
    internal static List<StackDto> ToFullDto(List<StackDto> stacks)
    {
        if (!stacks.Any()) return stacks;

        var count = stacks.Count;

        for (var i = 0; i < count; i++) stacks[i].Number = i + 1;

        return stacks;
    }
}