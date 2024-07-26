namespace FlashcardsProgram.Utils;

public class Text
{
    public static string Error(string text)
    {
        return $"[red]{text}[/]";
    }

    public static string Markup(string text, string tag)
    {
        return $"[{tag}]{text}[/]";
    }
}