using Dapper;
using Microsoft.Data.SqlClient;

class StudySessionDatabaseManager : DataBaseManager
{
    protected override string TableName => "study_sessions";
    public override async Task BuildTable()
    {
        var sql = 
        $@"CREATE TABLE {TableName} (
            Stacks_Id INTEGER NOT NULL
            FOREIGN KEY (Stacks_Id) REFERENCES stacks (Id),
            Id INTEGER,
            Date TEXT,
            Score INTEGER
        )";

        await HandleDatabaseOperation(async (connection) => {
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }, 
        $"Loading {TableName}",
        $"[bold red]{TableName} table does not exist. Creating new table...[/]"
        );
    }

    public async Task<List<StudySession>> GetLogs(Stack stack)
    {
        List<StudySession> result = [];
        string sql = $"SELECT * FROM {TableName} WHERE Stacks_Id = {stack.Id} ORDER BY Id";
        
        await HandleDatabaseOperation(async (connection) => {
            result = (List<StudySession>) await connection.QueryAsync<StudySession>(sql);
        },
        $"Retrieving logs",
        $"[bold green]Logs retrieved[/]"
        );

        return result;
    }

    public override async Task InsertLog(Data data)
    {
        await HandleDatabaseOperation(async (connection) => {
            StudySession studySession = ValidateDataType<StudySession>(data) ?? 
                throw new ArgumentException("Data is not of Flashcard");

            var sql = 
            $@"INSERT INTO {TableName} VALUES (
                {studySession.Stacks_Id},
                {studySession.Id},
                '{studySession.Date}',
                {studySession.Score}
            )";
            
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }, 
        $"Inserting log",
        $"[bold green]New log added to {TableName}[/]"
        );
    }

   
}