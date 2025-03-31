using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

class DataBaseManager
{
    static string ConnectionString = "";
    public static async Task StartAsync()
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

        await BuildTable("stacks",
        [
            "Id INTEGER PRIMARY KEY",
            "Name TEXT"
        ]);

        await BuildTable("flash_cards",
        [
            "Stacks_Id INTEGER NOT NULL",
            "FOREIGN KEY (Stacks_Id) REFERENCES stacks (Id)",
            "Id INTEGER PRIMARY KEY",
            "Front TEXT",
            "Back TEXT"
        ]);
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
        ErrorCodes.TABLEEXISTS);
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
        ErrorCodes.INSERTLOGEXISTS);
    }

    public static async Task<List<T>> GetAllLogs<T>(string tableName)
    {
        List<T> result = [];
        await HandleDatabaseOperation(async (connection) => {
            var sql = $@"SELECT * FROM {tableName}";
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
    static async Task HandleDatabaseOperation(Func<SqlConnection, Task> method, ErrorCodes.ErrorCode errorCode = default)
    {
        try
        {
            await using SqlConnection connection = new(ConnectionString);
            await connection.OpenAsync();

            await method(connection);
        }
        catch (SqlException e) 
        { 
            if (e.Number == errorCode.Id) 
                AnsiConsole.MarkupLine(e.Number + " " + errorCode.Message); 
        }
        catch (Exception e) { Console.WriteLine(e); } // global catch for if i missed anything
    }
}