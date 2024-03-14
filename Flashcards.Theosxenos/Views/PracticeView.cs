namespace Flashcards.Views;

public class PracticeView : BaseView
{
    public void ShowLog(IEnumerable<dynamic> pivotData, int year)
    {
        var data = pivotData.ToList();

        var table = new Table();
        table.Title = new TableTitle($"Average scores for the year [bold]{year}[/]");

        var keys = ((IDictionary<string, object>)data[0]).Keys;
        var columns = keys.Select(k => new TableColumn(int.TryParse(k, out var month) ? GetMonthName(month) : k));
        table.AddColumns(columns.ToArray());

        foreach (IDictionary<string, object?> row in data)
            table.AddRow(row.Select(pair => pair.Value?.ToString() ?? "-").ToArray());

        AnsiConsole.Write(table);

        Console.ReadKey();
    }

    private string GetMonthName(int month, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        return culture.DateTimeFormat.GetMonthName(month);
    }

    public int GetYear(List<int> years)
    {
        return ShowMenu(years, "Please choose a year to get logs from:");
    }
}