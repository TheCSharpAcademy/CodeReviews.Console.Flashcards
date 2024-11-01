using Spectre.Console;
namespace FlashcardsLibrary.Views;

internal static class DataViewer
{
    internal static void DisplayHeader(string header, string justify="center")
    {
        var heading = new Rule($"[red]{header}[/]");
        switch(justify)
        {
            case "left":
                heading.Justification = Justify.Left;
                AnsiConsole.Write(heading);
                break;

            case "right":
                heading.Justification = Justify.Right;
                AnsiConsole.Write(heading);
                break;
            
            default:
                AnsiConsole.Write(heading);
                break;
        }
        System.Console.WriteLine();
    }

    public static void DisplayListAsTable<T>(string[] headers, List<T> data)
    {
        Table table = new();

        foreach (string header in headers)
            table.AddColumns(header);
        
        foreach(var item in data)
        {
            List<string> row = new List<string>();

            foreach(var header in headers)
            {
                var property = typeof(T).GetProperty(header);
                row.Add(property.GetValue(item).ToString() ?? "N/A");
            }
            table.AddRow(row.ToArray());
        }

        AnsiConsole.Write(table);
        System.Console.WriteLine();
    }

    public static void Figlet(string text, string justify="left", string color = "red")
    {
        Justify justification = justify switch 
        {
            "left" => Justify.Left,
            "right" => Justify.Right,
            _ => Justify.Center
        };

        Color colour = color switch 
        {
            "blue" => Color.Blue,
            "green" => Color.Green,
            "yellow" => Color.Yellow,
            _ => Color.Red
        };

        AnsiConsole.Write(
            new FigletText(text)
                .Justify(justification)
                .Color(colour)
        );
    }
}

