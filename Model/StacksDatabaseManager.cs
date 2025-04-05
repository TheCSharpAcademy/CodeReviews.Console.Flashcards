using Dapper;
using Microsoft.Data.SqlClient;

class StacksDatabaseManager : DataBaseManager
{
    protected override string TableName => "stacks";
    public override async Task BuildTable()
    {
        var sql = 
        $@"CREATE TABLE {TableName} (
            Id INTEGER IDENTITY(1,1) PRIMARY KEY,
            Name TEXT
        )";

        await HandleDatabaseOperation(async (connection) => {
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }, 
        $"Loading {TableName}",
        $"[bold red]{TableName} table does not exist. Creating new table...[/]"
        );
    }

    public override async Task<List<Stack>> GetLogs()
    {
        List<Stack> result = [];
        string sql = $"SELECT * FROM {TableName} ORDER BY Id";
        
        await HandleDatabaseOperation(async (connection) => {
            result = (List<Stack>) await connection.QueryAsync<Stack>(sql);
        },
        $"Retrieving logs",
        $"[bold green]Logs retrieved[/]"
        );

        return result;
    }

    public override async Task InsertLog(Data data)
    {
        await HandleDatabaseOperation(async (connection) => {
            Stack stack = ValidateDataType<Stack>(data) ?? 
                throw new ArgumentException("Data is not of Stack");

            var sql = 
            $@"INSERT INTO {TableName} VALUES (
                '{stack.Name}'
            )";
            
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }, 
        $"Inserting log",
        $"[bold green]New log added to {TableName}[/]"
        );
    }

    public override async Task UpdateLog(Data data)
    {   
        await HandleDatabaseOperation(async (connection) => {
            Stack stack = ValidateDataType<Stack>(data) ?? 
                throw new ArgumentException("Data is not of Stack");

            var sql = 
            $@"UPDATE {TableName} 
                SET Name = '{stack.Name}'
                WHERE Id = {stack.Id}";

            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        },
        $"Updating logs",
        $"[bold green]Log updated[/]"
        );
    }

}