using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace FlashCards;

public class DapperHelper
{
    private SqlConnection _connection;
    private readonly IConfiguration _config;
    private readonly ILogger _logger;
    private const int TextSize = 256;

    public DapperHelper(IConfiguration config, ILogger logger)
    {
        _config = config;
        _logger = logger;
        _connection = new SqlConnection(config.GetConnectionString("LocalDB"));
        
        _logger.Information("CONNECTION STRING: {0}", _connection.ConnectionString);
    }

    private void Log(string sql)
    {
        _logger.Information("EXECUTING SQL: {0}", sql);
    }

    public void InitializeDatabase()
    {
        _connection = new SqlConnection(_config.GetConnectionString("LocalDB"));

        if (_config["UseExampleData"] is "True")
        {
            DropTable("Cards");
            DropTable("StudySessions");
            DropTable("Stacks");
            CreateStacksTable();
            CreateCardsTable();
            CreateStudySessionsTable();
            PopulateDatabase();
        }
        else
        {
            CreateStacksTable();
            CreateCardsTable();
        }
    }

    private void PopulateDatabase()
    {
        var sql = $"""
                   -- Insert example data into Stacks table
                   INSERT INTO Stacks (Name) VALUES ('Mathematics');
                   INSERT INTO Stacks (Name) VALUES ('Science');
                   INSERT INTO Stacks (Name) VALUES ('History');
                   
                   -- Insert example data into Cards table for Mathematics
                   INSERT INTO Cards (Front, Back, StackId) VALUES ('What is 2+2?', '4', 1);
                   INSERT INTO Cards (Front, Back, StackId) VALUES ('What is the square root of 16?', '4', 1);
                   INSERT INTO Cards (Front, Back, StackId) VALUES ('What is the value of pi (π) to two decimal places?', '3.14', 1);
                   
                   -- Insert example data into Cards table for Science
                   INSERT INTO Cards (Front, Back, StackId) VALUES ('What is the chemical symbol for water?', 'H2O', 2);
                   INSERT INTO Cards (Front, Back, StackId) VALUES ('What planet is known as the Red Planet?', 'Mars', 2);
                   INSERT INTO Cards (Front, Back, StackId) VALUES ('What is the speed of light in millions of meters per second?', '300', 2);
                   
                   -- Insert example data into Cards table for History
                   INSERT INTO Cards (Front, Back, StackId) VALUES ('Who was the first president of the United States?', 'Washington', 3);
                   INSERT INTO Cards (Front, Back, StackId) VALUES ('In which year did the Titanic sink?', '1912', 3);
                   INSERT INTO Cards (Front, Back, StackId) VALUES ('What ancient civilization built the pyramids?', 'The Egyptians', 3);
                   
                   -- Insert example data into StudySessions table
                   INSERT INTO StudySessions (Date, Correct, Total, StackId) VALUES ('2024-08-18', 3, 5, 1);
                   INSERT INTO StudySessions (Date, Correct, Total, StackId) VALUES ('2024-08-19', 10, 11, 2);
                   INSERT INTO StudySessions (Date, Correct, Total, StackId) VALUES ('2024-08-20', 7, 10, 3);
                   INSERT INTO StudySessions (Date, Correct, Total, StackId) VALUES ('2024-08-21', 4, 5, 1);
                   INSERT INTO StudySessions (Date, Correct, Total, StackId) VALUES ('2024-08-22', 4, 14, 2);
                   INSERT INTO StudySessions (Date, Correct, Total, StackId) VALUES ('2024-08-23', 10, 10, 3);
                   INSERT INTO StudySessions (Date, Correct, Total, StackId) VALUES ('2024-08-24', 5, 5, 1);
                                       
                   
                   """;

        Log(sql);
        _connection.Execute(sql);
    }


    public void Dispose()
    {
        _connection.Dispose();
    }

    public bool AddStack(string name)
    {
        const string sql = "INSERT INTO Stacks (Name) VALUES (@name)";
        Log(sql);
        try
        {
            var rows = _connection.Execute(sql, new { name });
            return rows == 1;
        }
        catch (SqlException)
        {
            // Name already exists
            return false;
        }
        
    }

    public List<Stack> GetStacks()
    {
        const string sql = "SELECT * FROM Stacks";
        Log(sql);
        var stacks = _connection.Query<Stack>(sql).ToList();
        return stacks;
    }

    public Stack GetStackByName(string name)
    {
        const string sql = "SELECT * FROM Stacks WHERE Name = @name";
        Log(sql);
        var stack = _connection.QuerySingle<Stack>(sql, new { name });
        return stack;
    }

    public bool DeleteStack(int id)
    {
        const string sql = "DELETE FROM Stacks WHERE Id = @id";
        Log(sql);
        var rows = _connection.Execute(sql, new { id });
        return rows == 1;
    }

    public bool AddCard(string front, string back, string stack)
    {
        const string sql = "INSERT INTO Cards (Front, Back, StackId) VALUES (@front, @back, @stackId)";
        Log(sql);
        var stackId = GetStackByName(stack).Id;
        var rows = _connection.Execute(sql, new { front, back, stackId });
        return rows == 1;
    }

    public List<Card> GetCardsByStackId(int stackId)
    {
        const string sql = """
                           SELECT * FROM Cards WHERE StackId = @stackId
                           """;
        
        var results = _connection.Query<Card>(sql, new { stackId }).ToList();
        return results;
    }

    public List<CardDto> GetCardDtos()
    {
        const string sql = """
                           SELECT Cards.Id, Cards.Front, Cards.Back, Cards.StackId, Stacks.Name 
                           FROM Cards JOIN Stacks ON Cards.StackId=Stacks.Id
                           """;
        
        var results = _connection.Query<CardDto>(sql).ToList();
        return results;
    }

    public bool DeleteCard(int id)
    {
        const string sql = "DELETE FROM Cards WHERE Id = @id";
        Log(sql);
        var rows = _connection.Execute(sql, new { id });
        return rows == 1;
    }

    public bool AddSession(StudySession session)
    {
        const string sql = """
                           INSERT INTO StudySessions (Date, Correct, Total, StackId)
                           VALUES (@date, @correct, @total, @stackId)
                           """;
        Log(sql);
        var rows = _connection.Execute(sql, session);
        return rows == 1;
    }

    public List<StudySessionDto> GetSessions(int stack = 0)
    {
        var sql = """
                  SELECT Date, Correct, Total, Name
                  FROM StudySessions JOIN Stacks on StudySessions.StackId = Stacks.Id
                  """;

        if (stack > 0)
        {
            sql += $" WHERE StackId = {stack}";
        }
        
        var results = _connection.Query<StudySessionDto>(sql).ToList();
        return results;
    }

    private void CreateStacksTable()
    {
        var sql = $"""
                   IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks')
                   BEGIN
                       CREATE TABLE Stacks (
                           Id INT IDENTITY(1,1) PRIMARY KEY,
                           Name VARCHAR({TextSize}) NOT NULL UNIQUE,
                       )
                   END
                   """;
        
        Log(sql);
        _connection.Execute(sql);
    }

    private void CreateCardsTable()
    {
        var sql = $"""
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Cards')
                    BEGIN
                        CREATE TABLE Cards (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Front VARCHAR({TextSize}) NOT NULL,
                            Back VARCHAR({TextSize}),
                            StackId INT NOT NULL FOREIGN KEY REFERENCES Stacks(ID) ON DELETE CASCADE
                        )
                    END
                   """;
        
        Log(sql);
        _connection.Execute(sql);
    }

    private void CreateStudySessionsTable()
    {
        var sql = $"""
                   IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudySessions')
                   BEGIN
                        CREATE TABLE StudySessions (
                            ID INT IDENTITY(1,1) PRIMARY KEY,
                            Date CHAR(10) NOT NULL,
                            Correct INTEGER NOT NULL,
                            Total INTEGER NOT NULL,
                            StackId INT NOT NULL FOREIGN KEY REFERENCES Stacks(ID) ON DELETE CASCADE
                        )
                   END
                   """;
        
        Log(sql);
        _connection.Execute(sql);
    }

    private void DropTable(string name)
    {
        var sql = $"""
                   IF OBJECT_ID(N'{name}', N'U') IS NOT NULL
                   BEGIN
                        DROP TABLE {name}
                   END
                   """;
        
        _connection.Execute(sql);
    }
}