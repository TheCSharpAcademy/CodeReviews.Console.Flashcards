using Dapper;
using Microsoft.Data.SqlClient;

class FlashcardsDatabaseManager : DataBaseManager
{
    protected override string TableName => "flash_cards";
    public override async Task BuildTable()
    {
        var sql = 
        $@"CREATE TABLE {TableName} (
            Stacks_Id INTEGER NOT NULL
            FOREIGN KEY (Stacks_Id) REFERENCES stacks (Id),
            Id INTEGER,
            Front TEXT,
            Back TEXT
        )";

        await HandleDatabaseOperation(async (connection) => {
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }, 
        $"Loading {TableName}",
        $"[bold red]{TableName} table does not exist. Creating new table...[/]"
        );
    }

    public async Task<List<Flashcard>> GetLogs(Stack stack)
    {
        List<Flashcard> result = [];
        string sql = $"SELECT * FROM {TableName} WHERE Stacks_Id = {stack.Id} ORDER BY Id";
        
        await HandleDatabaseOperation(async (connection) => {
            result = (List<Flashcard>) await connection.QueryAsync<Flashcard>(sql);
        },
        $"Retrieving logs",
        $"[bold green]Logs retrieved[/]"
        );

        return result;
    }

    public override async Task InsertLog(Data data)
    {
        await HandleDatabaseOperation(async (connection) => {
            Flashcard flashcard = ValidateDataType<Flashcard>(data) ?? 
                throw new ArgumentException("Data is not of Flashcard");

            var sql = 
            $@"INSERT INTO {TableName} VALUES (
                {flashcard.StacksId},
                {flashcard.Id},
                '{flashcard.Front}',
                '{flashcard.Back}'
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
            Flashcard flashcard = ValidateDataType<Flashcard>(data) ?? 
                throw new ArgumentException("Data is not of flashcard");

            var sql = 
            $@"UPDATE {TableName} 
                SET Stacks_Id = {flashcard.StacksId},
                    Front = '{flashcard.Front}',
                    Back = '{flashcard.Back}'
                WHERE Id = {flashcard.Id}";

            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        },
        $"Updating logs",
        $"[bold green]Log updated[/]"
        );
    }

    public async Task UpdateIds(int id)
    {
        await HandleDatabaseOperation(async (connection) => {
            var sql = 
            $@"UPDATE {TableName}
                SET Id = Id - 1
                WHERE Id in (SELECT Id FROM flash_cards WHERE Id > {id})";
            
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        },
        $"Updating logs",
        $"[bold green]Log updated[/]"
        );
    }
}