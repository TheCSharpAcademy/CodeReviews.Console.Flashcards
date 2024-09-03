using Spectre.Console;
using System.Reflection;
using System.Text;

namespace Flashcards;

public static class OutputRenderer
{
    private static readonly Dictionary<Type, PropertyInfo[]> PropertiesCache = new Dictionary<Type, PropertyInfo[]>();


    /// <summary>
    /// Displays a list of objects in a table format.
    /// </summary>
    /// <param name="values">List of objects to display.</param>
    /// <typeparam name="T">Type of object, must be a class.</typeparam>

    public static void ShowTable<T>(List<T> values, string title) where T : class
    {
        // Create a table
        var table = new Table();

        // Set the table border and style options
        table.Border(TableBorder.Rounded);
        table.BorderColor(Color.LightCoral);
        table.Title($"[bold yellow]{title}[/]");
        table.Caption("Generated on " + DateTime.Now.ToString("g"));

        PropertyInfo[] properties = GetCachedProperties<T>();

        // Add columns
        foreach (PropertyInfo property in properties)
        {
            table.AddColumn(new TableColumn(new Markup("[bold yellow]" + property.Name + "[/]")).Centered().PadRight(2));
        }

        // Add rows
        foreach (T value in values)
        {
            List<string> row = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                string? propertyValue = property.GetValue(value)?.ToString();
                row.Add(propertyValue ?? "N/A");
            }
            table.AddRow(row.ToArray());
        }

        AnsiConsole.Write(table);
    }

    // Cache to avoid performance issues due to reflection
    private static PropertyInfo[] GetCachedProperties<T>() where T : class
    {
        Type type = typeof(T);

        if (!PropertiesCache.TryGetValue(type, out PropertyInfo[] properties))
        {
            properties = type.GetProperties();
            PropertiesCache.Add(type, properties);
        }

        return properties;
    }
}