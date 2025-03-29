using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Spectre.Console;

class DataBaseManager
{
    static string ConnectionString = "";
    public static void Start()
    {
        var builder = new SqlConnectionStringBuilder
            {
                DataSource = "localhost",
                UserID = "sa",
                Password = "<YourStrong@Passw0rd>",
                InitialCatalog = "FlashCardsProject",
                TrustServerCertificate=true
            };
        ConnectionString = builder.ConnectionString;     
    }

    public static async Task BuildTable(string tableName, List<string> optionsList)
    {
        await HandleDatabaseOperation(async (connection) => {
            string optionsString = string.Join(",", optionsList);
            var sql = $@"CREATE TABLE {tableName}({optionsString})";
            
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();

            AnsiConsole.MarkupLine($"[bold red]{tableName} table does not exist. Creating new table...[/]");
        }, 
        ErrorCodes.TABLEEXISTS, $"[bold green]{tableName} table already exists[/]");
    }

    public static async Task InsertLog()
    {
        await HandleDatabaseOperation(async (connection) => {
            var sql = 
            $@"INSERT INTO stacks
            VALUES (0, 'neat')";

            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();

            AnsiConsole.MarkupLine("[bold green]New log added[/]");
        }, 
        ErrorCodes.INSERTLOGEXISTS, "[bold red]Log already exists[/]");
    }

    public static async Task GetAllLogs()
    {
        await HandleDatabaseOperation(async (connection) => {
            var sql = "SELECT * FROM stacks";
            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Console.WriteLine("{0} {1}", reader.GetValue(0), reader.GetValue(1));
            }
        });
    }

    // Handles the methods that deal with the db. The passed method
    // (or more realistically lambda function) needs an SqlConnection
    // (made in the try statment) and will return Task, allowing it
    // to be an async method/lambda. 
    static async Task HandleDatabaseOperation(Func<SqlConnection, Task> method, int errorCode = default, string message = default)
    {
        try
        {
            await using SqlConnection connection = new(ConnectionString);
            await connection.OpenAsync();

            await method(connection);
        }
        catch (SqlException e) { if (e.ErrorCode == errorCode) AnsiConsole.MarkupLine(message); }
        catch (Exception e) { Console.WriteLine(e); } // global catch for if i missed anything
    }
}