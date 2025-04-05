using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

abstract class DataBaseManager
{
    protected virtual string TableName => "";
    private static string ConnectionString = "";

    public static void Start()
    {
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

    public virtual Task BuildTable(){ return Task.CompletedTask; }

    public virtual Task InsertLog(Data data){ return Task.CompletedTask; }
    
    public virtual Task GetLogs(){ return Task.CompletedTask; }

    public virtual Task UpdateLog(Data data){ return Task.CompletedTask; }

    public virtual async Task DeleteLog(int id)
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

    // Technically useless now since funcs are now virtual instead of 
    // abstract, but may be needed for potential future implementations
    protected static T? ValidateDataType<T>(Data data)
    {
        switch (data)
        {
            case T isValidType:
                return isValidType;
            default:
                return default;
        }
    }

    // Handles the methods that deal with the db. The passed method
    // (or more realistically lambda function) needs an SqlConnection
    // (made in the try statment) and will return Task, allowing it
    // to be an async method/lambda. 
    protected async Task HandleDatabaseOperation(Func<SqlConnection, Task> method, 
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