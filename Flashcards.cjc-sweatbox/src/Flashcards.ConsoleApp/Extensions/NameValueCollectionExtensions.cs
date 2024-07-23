using System.Collections.Specialized;

namespace Flashcards.ConsoleApp.Extensions;

/// <summary>
/// NameValueCollection class extension methods.
/// </summary>
public static class NameValueCollectionExtensions
{
    #region Methods

    public static bool GetBoolean(this NameValueCollection collection, string key)
    {
        string? value = collection.Get(key);

        if (value == null)
        {
            throw new Exception($"Unable to get a value for '{key}' in the collection.");
        }

        if (!bool.TryParse(value, out bool result))
        {
            throw new Exception($"Unable to parse '{value}' to boolean for '{key}' in the collection.");
        }

        return result;
    }

    public static string GetString(this NameValueCollection collection, string key)
    {
        string? value = collection.Get(key);

        if (value == null)
        {
            throw new Exception($"Unable to get a value for '{key}' in the collection.");
        }

        return value;
    }

    #endregion
}
