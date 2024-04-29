using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

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
        SeedData();
    }

    public void CreateTables()
    {
        _stacksTableClass.CreateTable();
        _flashCardsTableClass.CreateTable();
        _studySessionTableClass.CreateTable();
    }

    private void SeedData()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute("INSERT INTO Stacks (Name) VALUES ('Capitals')");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('UK Capital ?', 'London', 1000, 1) ");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('India Capital?', 'Delhi', 1000, 2) ");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('USA Capital?', 'Washington D.C', 1000, 3) ");
        conn.Execute("INSERT INTO StudySessions(Fk_StackID, SessionDate, SessionScore) VALUES (1000, '2024-04-29', 1)");
        conn.Execute("INSERT INTO StudySessions(Fk_StackID, SessionDate, SessionScore) VALUES (1000, '2024-04-29', 3)");

        conn.Execute("INSERT INTO Stacks (Name) VALUES ('Currency')");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('UK Currency ?', 'Pound', 1001, 1) ");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('India Currency ?', 'Rupees', 1001, 2) ");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('Japan Currency?', 'Yen', 1001, 3) ");
        conn.Execute("INSERT INTO StudySessions(Fk_StackID, SessionDate, SessionScore) VALUES (1001, '2024-04-30', 0)");

        conn.Execute("INSERT INTO Stacks (Name) VALUES ('National Animals')");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('India National Animal ?', 'BengalTiger', 1002, 1) ");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('Canada National Animal?', 'Beaver', 1002, 2) ");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('Australia National Animal??', 'Kangaroo', 1002, 3) ");
    }

}