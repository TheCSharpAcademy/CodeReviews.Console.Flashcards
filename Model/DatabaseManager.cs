using Dapper;
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

    public static async Task InsertLog(string table, List<string> valuesList)
    {
        await HandleDatabaseOperation(async (connection) => {
            string values = string.Join(",", valuesList);
            var sql = 
            $@"INSERT INTO {table}
            VALUES ({values})";

            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();

            AnsiConsole.MarkupLine($"[bold green]New log added to {table}[/]");
        }, 
        ErrorCodes.INSERTLOGEXISTS, $"[bold red]Log already exists[/]");
    }

    public static async Task<List<T>> GetAllLogs<T>(string table)
    {
        List<T> result = [];
        await HandleDatabaseOperation(async (connection) => {
            var sql = $@"SELECT * FROM {table}";
            //await using var command = new SqlCommand(sql, connection);
            result = (List<T>) await connection.QueryAsync<T>(sql);
        });

        return result;
    }

    public static async Task UpdateLog(int id, string tableName, List<string> valuesList)
    {
        await HandleDatabaseOperation(async (connection) => {
            string values = string.Join(",", valuesList);
            var sql = $@"UPDATE {tableName} SET {values} WHERE Id = {id}";


            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();

            AnsiConsole.MarkupLine($"[bold green]{tableName} has been updated[/]");
        });
    }

    public static async Task DeleteLog(int id, string tableName)
    {
        await HandleDatabaseOperation(async (connection) => {
            var sql = $@"DELETE FROM {tableName} WHERE Id = {id}";

            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
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