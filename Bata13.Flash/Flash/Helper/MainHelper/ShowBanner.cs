using Spectre.Console;

namespace Flash.Helper.MainHelper;
internal class ShowBanner
{
    internal static void GetShowBanner(string figletText, Color figletColor)
    {
        AnsiConsole.Write
            (
                new FigletText(figletText)
                    .LeftJustified()
                    .Color(figletColor)
            );

        var panel = new Panel("What Would You Like to Do?");

        AnsiConsole.Write(
                new Panel(panel)
                    .Header("")
                    .Collapse()
                    .RoundedBorder()
                    .BorderColor(Color.White));
    }
}
