using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using AdityaFlashCards.Database.Models;

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
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            if (!AreTablesCreated(conn))
            {
                _stacksTableClass.CreateTable();
                _flashCardsTableClass.CreateTable();
                _studySessionTableClass.CreateTable();
                SeedData();
            }
        }
    }

    private bool AreTablesCreated(SqlConnection conn)
    {
        string sql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND (TABLE_NAME = 'Stacks' OR TABLE_NAME = 'FlashCards' OR TABLE_NAME = 'StudySessions')";
        int count = conn.QuerySingle<int>(sql);
        return count == 3; 
    }


    private void SeedData()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute("INSERT INTO Stacks (Name) VALUES ('Capitals')");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('UK Capital ?', 'London', 1000, 1) ");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('India Capital?', 'Delhi', 1000, 2) ");
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES ('Russia Capital?', 'Moscow', 1000, 3) ");
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

    public List<Stack> GetAllStacks()
    {
        List<Stack> AllStacks = _stacksTableClass.GetAllStacks();
        return AllStacks;
    }

    public void CreateNewStack(string name)
    {
        _stacksTableClass.InsertNewStack(name);
    }

    public void DeleteStack(string name)
    {
        _stacksTableClass.DeleteStack(name);
    }

    public List<StudySession> GetStudySessions()
    {
        return _studySessionTableClass.GetAllStudySessions();
    }

    public List<FlashCardDTOFlashCardView> GetFlashCards()
    {
        return _flashCardsTableClass.GetFlashCards();
    }

    public List<FlashCardDTOStackView> GetFlashCardsForGivenStack(string name)
    {
        return _flashCardsTableClass.GetFlashCardsForGivenStack(name);
    }

    public bool IsFlashCardIdPresent(int id)
    {
        return _flashCardsTableClass.IsFlashCardIdPresent(id);
    }

    public void UpdateFlashCard(int flashCardId, string question, string answer)
    {
        _flashCardsTableClass.UpdateFlashCard(flashCardId, question, answer);
    }

    public void DeleteFlashCard(int flashCardId)
    {
        (int fK_StackID, int positionInStack) = _flashCardsTableClass.GetStackIDAndPosition(flashCardId);
        _flashCardsTableClass.DeleteFlashCard(fK_StackID, positionInStack);
    }

    public void CreateFlashCard(string stackName, string question, string answer)
    {
        int fK_StackID = _stacksTableClass.GetStackIdFromStackName(stackName);
        int positionInStack = _flashCardsTableClass.GetLastPositionInStack(fK_StackID);
        _flashCardsTableClass.InsertFlashCard(question, answer, fK_StackID, positionInStack+ 1);
    }


}