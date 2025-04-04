using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

class DataBaseManager<T>
{
    public static string TableName = "";
    static string ConnectionString = "";

    public static void Start(string tableName)
    {
        TableName = tableName;

        var builder = new SqlConnectionStringBuilder
        {
            DataSource = "localhost",
            UserID = "sa",
            Password = "<YourStrong@Passw0rd>",
            InitialCatalog = "FlashCardsProject",
            TrustServerCertificate = true
        };
        ConnectionString = builder.ConnectionString;
    }

    public static async Task BuildTable(List<string> optionsList)
    {
        string optionsString = string.Join(",", optionsList);
        var sql = $@"CREATE TABLE {TableName}({optionsString})";

        await HandleDatabaseOperation(async (connection) => {
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }, 
        $"Loading {TableName}",
        $"[bold red]{TableName} table does not exist. Creating new table...[/]"
        );
    }

    // For flash cards: Stack_Id, Id, Front, Back
    // For stacks: Id, Name
    public static async Task InsertLog(params object[] valuesInput)
    {
        List<string> parsedValuesList = ParseValues(valuesInput);
        string valuesString = string.Join(",", parsedValuesList);

        var sql = "INSERT INTO " + TableName + " VALUES ("+ valuesString +")";

        await HandleDatabaseOperation(async (connection) => {
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }, 
        $"Inserting log",
        $"[bold green]New log added to {TableName}[/]"
        );
    }

    static List<string> ParseValues(params object[] valuesInput)
    {
        List<string> parsedValuesList = [];
        foreach (var value in valuesInput)
        {
            switch (value)
            {
                case int Int:
                    parsedValuesList.Add(Int.ToString());
                    break;
                case string Str:
                    parsedValuesList.Add("'" + Str + "'");
                    break;
            }
        }

        return parsedValuesList;
    }

    public static async Task<List<T>> GetLogs(string query = "")
    {
        List<T> result = [];
        if (query != "")
            query = " WHERE " + query;
        string sql = "SELECT * FROM " + TableName + query +" ORDER BY Id";
        
        await HandleDatabaseOperation(async (connection) => {
            result = (List<T>) await connection.QueryAsync<T>(sql);
        },
        $"Retrieving logs",
        $"[bold green]Logs retrieved[/]"
        );

        return result;
    }

    public static async Task UpdateLog(string query, List<string> valuesList)
    {
        string values = string.Join(",", valuesList);
        var sql = $@"UPDATE {TableName} SET {values} WHERE {query}";

        await HandleDatabaseOperation(async (connection) => {
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        },
        $"Updating logs",
        $"[bold green]Table has been updated[/]"
        );
    }

    public static async Task DeleteLog(int id)
    {
        var sql = $@"DELETE FROM {TableName} WHERE Id = {id}";
        
        await HandleDatabaseOperation(async (connection) => {
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        },
        $"Deleting logs",
        $"[bold green]Log Deleted[/]"
        );
    }

    // Handles the methods that deal with the db. The passed method
    // (or more realistically lambda function) needs an SqlConnection
    // (made in the try statment) and will return Task, allowing it
    // to be an async method/lambda. 
    static async Task HandleDatabaseOperation(Func<SqlConnection, Task> method, 
                                                string loadingMessage, 
                                                string workedMessage)
    {
        try
        {
            await AnsiConsole.Status()
                .StartAsync(loadingMessage, async ctx => {
                    await using SqlConnection connection = new(ConnectionString);
                    await connection.OpenAsync();
                    await method(connection);
                });
            
            AnsiConsole.MarkupLine($"[bold grey]{TableName}:[/]" + workedMessage);
        }
        catch (SqlException e) 
        { 
            if (ErrorCodes.DBCodes.TryGetValue(e.Number, out string message))
                AnsiConsole.MarkupLine($"[bold grey]{TableName}:[/]" + message);
            else
                Console.WriteLine(e);
        }
        catch (Exception e) { Console.WriteLine(e); } // global catch for if i missed anything
    }
}