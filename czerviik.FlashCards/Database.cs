using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

namespace FlashCards;

public abstract class Database
{
    protected string _connectionString;
    protected string _fileName;
    protected Database(string connectionString, string fileName)
    {
        _connectionString = connectionString;
        _fileName = fileName;
        InitializeDatabase();

    }
    public abstract void InitializeDatabase();
    public abstract object GetById(int id);
    protected void ExecuteCommand(string sql)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            connection.Execute(sql);
        }
    }
}

public class FlashcardDb : Database
{
    public FlashcardDb(string connectionString, string fileName) : base(connectionString, fileName) { }

    public override void InitializeDatabase()
    {
        var sql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'flashcards' AND type = 'U')
                BEGIN
                    CREATE TABLE flashcards(
                        Id INT PRIMARY KEY IDENTITY(1,1), 
                        Question VARCHAR(50), 
                        Answer VARCHAR(50), 
                        StackId INT,
                        FOREIGN KEY(StackId) REFERENCES stacks(Id) ON DELETE CASCADE
                        );
                END";

        ExecuteCommand(sql);
    }


    private List<Flashcard> ReadRowsCommand(string sql, object parameters = null)
    {
        var flashcardsList = new List<Flashcard>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var flashcards = connection.Query<Flashcard>(sql, parameters);
            foreach (var flashcard in flashcards)
            {
                flashcardsList.Add(flashcard);
            }
            return flashcardsList;
        }
    }

    public void Insert(string question, string answer, int stackId)
    {
        var sql = @$"
        INSERT INTO flashcards (Question, Answer, StackId)
        VALUES ('{question}','{answer}',{stackId})";

        ExecuteCommand(sql);
    }
    public void Update(string question, string answer, int stackId, int id)
    {
        var sql = @$"
        UPDATE flashcards
        SET Question = '{question}', Answer = '{answer}', StackId = {stackId}
        WHERE Id = {id}";

        ExecuteCommand(sql);
    }

    public void Delete(int id)
    {
        var sql = @$"
        DELETE FROM flashcards
        WHERE Id = {id}";

        ExecuteCommand(sql);
    }

    public List<Flashcard> GetAll()
    {
        var sql = "SELECT * FROM flashcards";
        return ReadRowsCommand(sql);
    }

    public override Flashcard GetById(int id)
    {
        var sql = $"SELECT * FROM flashcards WHERE Id = @Id";

        using (var connection = new SqlConnection(_connectionString))
        {
            return connection.QuerySingleOrDefault<Flashcard>(sql, new { Id = id });
        }
    }

    public List<Flashcard> GetByStackId(int id)
    {
        var sql = $"SELECT * FROM flashcards WHERE StackId = @Id";
        return ReadRowsCommand(sql, new { Id = id });
    }
}


public class StackDb : Database
{
    public StackDb(string connectionString, string fileName) : base(connectionString, fileName) { }

    public override void InitializeDatabase()
    {
        var sql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'stacks' AND type = 'U')
                BEGIN
                    CREATE TABLE stacks( 
                        Id INT PRIMARY KEY IDENTITY(1,1), 
                        Name VARCHAR(50)
                    );
                END";

        ExecuteCommand(sql);
    }

    private List<Stack> ReadRowsCommand(string sql, object parameters = null)
    {
        var stacksList = new List<Stack>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var stacks = connection.Query<Stack>(sql, parameters);
            foreach (var stack in stacks)
            {
                stacksList.Add(stack);
            }
            return stacksList;
        }
    }

    private Stack ReadSingleCommand(string sql, object parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return connection.QuerySingleOrDefault<Stack>(sql, parameters);
        }
    }

    public void Insert(string stackName)
    {
        var sql = @$"
        INSERT INTO stacks (Name)
        VALUES ('{stackName}')";

        ExecuteCommand(sql);
    }

    public void Delete (int id)
    {
        var sql = @$"
        DELETE FROM stacks
        WHERE Id = {id}";

        ExecuteCommand(sql);
    }

    public List<Stack> GetAll()
    {
        var sql = "SELECT * FROM stacks";
        return ReadRowsCommand(sql);
    }

    public override List<Stack> GetById(int id)
    {
        var sql = $"SELECT * FROM stacks WHERE Id = @Id";
        return ReadRowsCommand(sql, new { Id = id });
    }

    public Stack GetByName(string name)
    {
        var sql = $"SELECT * FROM stacks WHERE Name = @Name";
        return ReadSingleCommand(sql, new { Name = name });
    }

    public bool NamePresent(string name)
    {
        var sql = $"SELECT * FROM stacks WHERE Name = @Name";
        return ReadSingleCommand(sql, new { Name = name }) != null;
    }

}
//vytvořit study session databázi a v Menu.cs zavést ukládání. Pak vytvořit funkci, jestli opakovat study session. Poté zobrazení study session, atd.