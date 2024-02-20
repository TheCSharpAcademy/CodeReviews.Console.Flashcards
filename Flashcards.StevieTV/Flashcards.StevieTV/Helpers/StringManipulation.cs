using System.Globalization;

namespace Flashcards.StevieTV.Helpers;

internal static class StringManipulation
{
    public static string ToTitleCase(this string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
    }
}