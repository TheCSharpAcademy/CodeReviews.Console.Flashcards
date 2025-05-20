using Spectre.Console;

namespace Flashcards.glaxxie.DTO;

internal record CardStyleFromConfig(int Width, int Height, int[] Padding);
internal record CardStyleDto(int Width, int Height, Padding Padding);
internal static class CardStyleDtoConverter
{
    internal static CardStyleDto ToCardStyleDto(this CardStyleFromConfig conf) =>
        new(conf.Width, conf.Height, new Padding(conf.Padding[0], conf.Padding[1], conf.Padding[2], conf.Padding[3]));
}