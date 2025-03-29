using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Spectre.Console;

class DataBaseManager
{
    // TODO: Figure out how async works
    // TODO: Figure out local connection var



    public static async Task Start()
    {
        var builder = new SqlConnectionStringBuilder
            {
                DataSource = "localhost",
                UserID = "sa",
                Password = "<YourStrong@Passw0rd>",
                InitialCatalog = "FlashCardsProject",
                TrustServerCertificate=true
            };
        var connectionString = builder.ConnectionString;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await BuildTable(connection, "stacks", new List<string>
        {
            "Id INTEGER PRIMARY KEY",
            "Name TEXT"
        });

        await BuildTable(connection, "flash_cards", new List<string>
        {
            "Stacks_Id INTEGER NOT NULL",
            "FOREIGN KEY (Stacks_Id) REFERENCES stacks (Id)",
            "Id INTEGER PRIMARY KEY",
            "Front TEXT",
            "Back TEXT"
        });

        await InsertLog(connection);

        await GetAllLogs(connection);
        
        await connection.CloseAsync();
    }

    static async Task BuildTable(SqlConnection connection, string tableName, List<string> optionsList)
    {
        await HandleDatabaseOperation(async () => {
            string optionsString = "";

            foreach(string option in optionsList)
            {
                optionsString += option + ",";
            }
            optionsString = optionsString.TrimEnd(',');

            var sql = $@"CREATE TABLE {tableName}({optionsString})";
            
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();

            AnsiConsole.MarkupLine($"[bold red]{tableName} table does not exist. Creating new table...[/]");
        }, 
        ErrorCodes.TABLEEXISTS, $"[bold green]{tableName} table already exists[/]");
    }

    static async Task InsertLog(SqlConnection connection)
    {
        await HandleDatabaseOperation(async () => {
            var sql = 
            $@"INSERT INTO stacks
            VALUES (0, 'neat')";

            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();

            AnsiConsole.MarkupLine("[bold green]New log added[/]");
        }, 
        ErrorCodes.INSERTLOGEXISTS, "[bold red]Log already exists[/]");
    }

    static async Task GetAllLogs(SqlConnection connection)
    {
        var sql = "SELECT * FROM stacks";
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Console.WriteLine("{0} {1}", reader.GetValue(0), reader.GetValue(1));
        }
    }

    static async Task HandleDatabaseOperation (Func<Task> method, int errorCode, string message)
    {
        try
        {
            await method();
        }
        catch (SqlException e) { if (e.ErrorCode == errorCode) AnsiConsole.MarkupLine(message); }
        catch (Exception e) { Console.WriteLine(e); } // global catch for if i missed anything
    }
}