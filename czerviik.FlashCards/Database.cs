using System;
using System.Collections.Generic;
using System.Linq;
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
                CREATE TABLE IF NOT EXISTS flashcards(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Question VARCHAR(50), 
                    Answer VARCHAR(50), 
                    StackId INT,
                    FOREIGN KEY(StackId) REFERENCES stacks(StackId))";

        ExecuteCommand(sql);
    }

    private List<Flashcard> ReadRowsCommand(string sql)
    {
        var flashcardsList = new List<Flashcard>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var flashcards = connection.Query<Flashcard>(sql);
            foreach (var flashcard in flashcards)
            {
                flashcardsList.Add(flashcard);
            }
            return flashcardsList;
        }
    }

    private Flashcard ReadSingleCommand(string sql, int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return connection.QuerySingleOrDefault(sql, new { Id = id });
        }
    }

    public void Insert(string question, string answer, int stackId)
    {
        var sql = @$"
        INSERT INTO flashcards (Question, Answer, StackId)
        VALUES ('{question}','{answer}',{stackId})";

        ExecuteCommand(sql);
    }

    public List<Flashcard> GetAll()
    {
        var sql = "SELECT * FROM flashcards";
        return ReadRowsCommand(sql);
    }

    public Flashcard GetFlashcard(int id)
    {
        var sql = $"SELECT * FROM flashcards WHERE Id = @Id";
        return ReadSingleCommand(sql, id);
    }
}

public class StackDb : Database
{
    public StackDb(string connectionString, string fileName) : base(connectionString, fileName) { }

    public override void InitializeDatabase()
    {
        var sql = @"
                CREATE TABLE IF NOT EXISTS stacks(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Name VARCHAR(50))";

        ExecuteCommand(sql);
    }

    private List<Stack> ReadRowsCommand(string sql)
    {
        var stacksList = new List<Stack>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var stacks = connection.Query<Stack>(sql);
            foreach (var stack in stacks)
            {
                stacksList.Add(stack);
            }
            return stacksList;
        }
    }

    public void Insert(string language)
    {
        var sql = @$"
        INSERT INTO stacks (Language)
        VALUES ('{language}')";

        ExecuteCommand(sql);
    }

    public List<Stack> GetAll()
    {
        var sql = "SELECT * FROM flashcards";
        return ReadRowsCommand(sql);
    }
}