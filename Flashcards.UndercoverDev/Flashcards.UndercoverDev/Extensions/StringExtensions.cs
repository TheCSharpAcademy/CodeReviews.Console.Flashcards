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
            if (str == null || str.Length == 0)
            {
                return str;
            }

            return char.ToUpper(str[0]) + str[1..];
        }
    }
}