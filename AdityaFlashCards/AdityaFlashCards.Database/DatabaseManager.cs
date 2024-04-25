using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace AdityaFlashCards.Database.DatabaseManager;

public class DatabaseManager
{
    private readonly string? _connectionString;
    private readonly StacksTableClass _stacksTableClass;
    private readonly FlashCardsTableClass _flashCardsTableClass;
    private readonly StudySessionTableClass _studySessionTableClass;


    public DatabaseManager(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _stacksTableClass = new StacksTableClass(_connectionString);
        _flashCardsTableClass = new FlashCardsTableClass(_connectionString);
        _studySessionTableClass = new StudySessionTableClass(_connectionString);
        CreateTables();
    }

    public void CreateTables()
    {
        //using SqlConnection conn = new SqlConnection(_connectionString);
        //conn.Open();
        //conn.Execute(@"CREATE TABLE MrVaidya1 (SessionId INTEGER PRIMARY KEY, SessionStartTime TEXT, SessionEndTime TEXT, SessionDuration TEXT,SessionCodingDate TEXT)");
        //Console.WriteLine("Table created");
        _stacksTableClass.CreateTable();
        //_flashCardsTableClass.CreateTable();
        //_studySessionTableClass.CreateTable();
    }


}