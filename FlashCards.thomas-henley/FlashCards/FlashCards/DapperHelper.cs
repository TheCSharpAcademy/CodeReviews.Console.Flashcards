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

        if (_config["UseExampleData"] is not null and "True")
        {
            _connection.Execute("DROP TABLE Cards");
            _connection.Execute("DROP TABLE Stacks");
            CreateStacksTable();
            CreateCardsTable();
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
                   
                   """;

        Log(sql);
        _connection.Execute(sql);
    }


    public void Dispose()
    {
        _connection.Dispose();
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

    public bool AddStack(string name)
    {
        const string sql = "INSERT INTO Stacks (Name) VALUES (@name)";
        Log(sql);
        try
        {
            var rows = _connection.Execute(sql, new { name });
            return rows == 1;
        }
        catch (SqlException ex)
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

    public List<CardDTO> GetCardDtos()
    {
        const string sql = """
                           SELECT Cards.Id, Cards.Front, Cards.Back, Cards.StackId, Stacks.Name 
                           FROM Cards JOIN Stacks ON Cards.StackId=Stacks.Id
                           """;
        
        var results = _connection.Query<CardDTO>(sql).ToList();
        return results;
    }

    public bool DeleteCard(int id)
    {
        const string sql = "DELETE FROM Cards WHERE Id = @id";
        Log(sql);
        var rows = _connection.Execute(sql, new { id });
        return rows == 1;
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
}