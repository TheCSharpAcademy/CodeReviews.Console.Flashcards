using System.Data;

namespace Flashcards.Data.Extensions;

/// <summary>
/// DateTime class extension methods.
/// </summary>
internal static class IDataReaderExtensions
{
    internal static DateTime GetDateTime(this IDataReader reader, string columnName)
    {
        return reader.GetDateTime(reader.GetOrdinal(columnName));
    }

    internal static int GetInt32(this IDataReader reader, string columnName)
    {
        return reader.GetInt32(reader.GetOrdinal(columnName));
    }

    internal static string GetString(this IDataReader reader, string columnName)
    {
        return reader.GetString(reader.GetOrdinal(columnName));
    }
}
