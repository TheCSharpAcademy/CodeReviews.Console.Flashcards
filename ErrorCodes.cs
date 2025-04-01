
class ErrorCodes
{
    public static readonly Dictionary<int, string> DBCodes = new()
    {
        { 0, "[bold red]Network error[/]" },
        { 102, "[bold red]Incorrect syntax[/]"},
        { 2714, "[bold green]Table already exists[/]" }, 
        { 2627, "[bold red]Log already exists[/]" },
    };
}