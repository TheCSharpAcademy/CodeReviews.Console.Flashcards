
class ErrorCodes
{
    public class ErrorCode(int id, string message)
    {
        public int Id { get; set; } = id;
        public string Message { get; set; } = message;
    }
    public static readonly ErrorCode TABLEEXISTS = new(2714, "[bold green]{tableName} table already exists[/]");
    public static readonly ErrorCode INSERTLOGEXISTS = new(2627, "[bold red]Log already exists[/]");
}