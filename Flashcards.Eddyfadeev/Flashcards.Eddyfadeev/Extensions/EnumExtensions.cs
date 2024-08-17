using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Flashcards.Eddyfadeev.Extensions;

/// <summary>
/// Provides extension methods for enumerations.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Retrieves the display name of an enum value using DisplayAttribute.
    /// If the display name is not defined, the name of the enum value is returned.
    /// </summary>
    /// <typeparam name="TEntry">The type of the enum</typeparam>
    /// <param name="enumValue">The enum value</param>
    /// <returns>The display name of the enum value</returns>
    public static string GetDisplayName<TEntry>(this TEntry enumValue) where TEntry : Enum
    {
        var displayName = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName();

        return displayName ?? enumValue.ToString();
    }

    /// <summary>
    /// Retrieves the display names of the enum values.
    /// </summary>
    /// <typeparam name="TEntry">The type of enum.</typeparam>
    /// <returns>An IEnumerable collection of strings representing the display names of the enum values.</returns>
    public static IEnumerable<string> GetDisplayNames<TEntry>() where TEntry : Enum
    {
        return Enum.GetValues(typeof(TEntry)).Cast<TEntry>().Select(e => e.GetDisplayName());
    }

    /// <summary>
    /// Retrieves the enum value from the display name.
    /// </summary>
    /// <typeparam name="TEntry">The enum type.</typeparam>
    /// <param name="displayName">The display name of the enum value.</param>
    /// <returns>The enum value corresponding to the display name.</returns>
    public static TEntry GetValueFromDisplayName<TEntry>(this string displayName) where TEntry : Enum
    {
        return Enum.GetValues(typeof(TEntry)).Cast<TEntry>().First(e => e.GetDisplayName() == displayName);
    }
}