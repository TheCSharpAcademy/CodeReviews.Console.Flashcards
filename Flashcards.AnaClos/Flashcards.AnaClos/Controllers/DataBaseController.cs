using Dapper;
using Microsoft.Data.SqlClient;

namespace Flashcards.AnaClos.Controllers;

public class DataBaseController
{   
    private string _connectionString;
    private string _dataBaseName;
    private string _server;

    public DataBaseController()
    {
        _server = System.Configuration.ConfigurationManager.AppSettings.Get("Server");
        _dataBaseName= System.Configuration.ConfigurationManager.AppSettings.Get("DataBase");
        _connectionString = $@"Server={_server};Integrated Security=true;TrustServerCertificate=true";
    }

    public void CreateDatabase()
    {
        string createDataBase = $@"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{_dataBaseName}')
                                BEGIN
                                CREATE DATABASE {_dataBaseName};
                                END";
        Execute(createDataBase);
    }

    public void CreateTableStacks()
    {
        string createTableStacks = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                                    BEGIN
                                    CREATE TABLE Stacks (
                                    Id INT PRIMARY KEY IDENTITY,
                                    Name NVARCHAR(50) NOT NULL,
                                    CONSTRAINT Stacks_Name UNIQUE(Name)
                                    );
                                    END";
        Execute(createTableStacks);
    }

    public void CreateTableFlashCards()
    {
        string createTableFlashCards = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FlashCards')
                                        BEGIN
                                        CREATE TABLE FlashCards (
                                        Id INT PRIMARY KEY IDENTITY,
                                        Front NVARCHAR(50) NOT NULL,
                                        Back NVARCHAR(50) NOT NULL,
                                        StackId INT NOT NULL,
                                        CONSTRAINT Front_Name UNIQUE(Front),
                                        CONSTRAINT fkStack FOREIGN KEY (StackId)
                                        REFERENCES Stacks(Id)
                                        ON DELETE CASCADE
                                        );
                                        END";
        Execute(createTableFlashCards);
    }

    public void CreateTableStudySessions()
    {
        string createTableFlashCards = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions')
                                        BEGIN
                                        CREATE TABLE StudySessions (
                                        Id INT PRIMARY KEY IDENTITY,
                                        Date DATETIME NOT NULL,
                                        Score INT NOT NULL,
                                        StackId INT NOT NULL
                                        );
                                        END";
        Execute(createTableFlashCards);
    }

    public void Execute(string commandText)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Execute(commandText);
        }
    }

    public int Execute<T>(string sql,T genericObject)
    {
        int rowsAffected = 0;
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                rowsAffected = connection.Execute(sql, genericObject);
            }
            catch (SqlException)
            {
                throw;
            }            
        }
        return rowsAffected;
    }
    public T QuerySingle<T>(string sql)
    {
        T value = default(T);
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                value = connection.QuerySingle<T>(sql);
            }
            catch (SqlException)
            {
                throw;
            }
        }
        return value;
    }     

    public List<T> Query<T>(string sql)
    {
        List<T> list = new List<T>();
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                list = connection.Query<T>(sql).ToList();
            }
            catch (SqlException)
            {
                throw;
            }
        }
        return list;
    }
}