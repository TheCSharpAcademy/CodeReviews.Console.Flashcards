using System.Globalization;

namespace Flashcards.UndercoverDev.Extensions
{
    public static class StringExtensions
    {
        public static string? TrimAndLower(this string str)
        {
            return str?.Trim().ToLower(); // Use null-conditional operator for safety
        }

        public static string? ToTitleCase(this string str)
        {
            if (str == null)
            {
                return null;
            }
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(str);
        }
    }
}