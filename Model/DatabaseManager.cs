using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Spectre.Console;

class DataBaseManager
{
    static readonly int ERRORCODETABLEEXISTS = -2146232060;
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

        await BuildStacksTable(connection);
        await BuildFlashCardsTable(connection);

        await GetAllLogs(connection);
        
        await connection.CloseAsync();
    }
    static async Task BuildStacksTable(SqlConnection connection)
    {
        try
        {
            var sql = 
            $@"CREATE TABLE stacks(
                Id INTEGER PRIMARY KEY,
                Name TEXT
                )";
            
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();

            AnsiConsole.MarkupLine("[bold red] flash_cards table did not exist. Creating new table...[/]");
        }
        catch (SqlException e) { if (e.ErrorCode == ERRORCODETABLEEXISTS) AnsiConsole.MarkupLine("[bold green]Table already exists[/]"); }
        catch (Exception e) { Console.WriteLine(e); }
    }

    static async Task BuildFlashCardsTable(SqlConnection connection)
    {
        try
        {
            var sql = 
            $@"CREATE TABLE flash_cards(
                Stacks_Id INTEGER NOT NULL,
                FOREIGN KEY (Stacks_Id)
                    REFERENCES stacks (Id),
                Id INTEGER PRIMARY KEY,
                Front TEXT,
                Back TEXT
                )";
            
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();

            AnsiConsole.MarkupLine("[bold red] flash_cards table did not exist. Creating new table...[/]");
        }
        catch (SqlException e) { if (e.ErrorCode == ERRORCODETABLEEXISTS) AnsiConsole.MarkupLine("[bold green]Table already exists[/]"); }
        catch (Exception e) { Console.WriteLine(e); }
    }

    static async Task GetAllLogs(SqlConnection connection)
    {
        var sql = "SELECT * FROM stacks";
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
        }
    }
}