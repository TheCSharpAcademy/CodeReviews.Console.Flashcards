using System.IO;
using Dapper;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace FlashCards.Tests;

[TestClass]
[TestSubject(typeof(DapperHelper))]
public class DapperHelperTest
{
    private IConfiguration _config;
    private ILogger _logger;
    private DapperHelper _dapper;
    private SqlConnection _testConnection;

    [ClassInitialize]
    public static void ClassInitialize(TestContext testContext)
    {
        LocalDbManager.CreateDatabase("FlashCardsTestDB");
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        LocalDbManager.DropDatabase("FlashCardsTestDB");
    }
    
    [TestInitialize]
    public void TestInitialize()
    {
        _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        _logger = new LoggerConfiguration()
            .ReadFrom.Configuration(_config)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        _dapper = new DapperHelper(_config, _logger);
        
        _testConnection = new SqlConnection(_config.GetConnectionString("LocalDB"));
    }

    [TestCleanup]
    public void TestCleanup()
    {
        if (IsTableCreated("Cards"))
            _testConnection.Execute("DROP TABLE Cards");
        if (IsTableCreated("Stacks"))
            _testConnection.Execute("DROP TABLE Stacks");
    }

    [TestMethod]
    public void InitializeDatabaseTest()
    {
        Assert.IsTrue(IsDatabaseCreated());
        Assert.IsFalse(IsTableCreated("Stacks"));
        Assert.IsFalse(IsTableCreated("Cards"));

        _dapper.InitializeDatabase();
        
        Assert.IsTrue(IsTableCreated("Stacks"));
        Assert.IsTrue(IsTableCreated("Cards"));
    }

    [TestMethod]
    public void AddStackTest()
    {
        _dapper.InitializeDatabase();
        Assert.AreEqual(0, GetStackCount());
        
        Assert.IsTrue(_dapper.AddStack("Stack 1"));
        Assert.AreEqual(1, GetStackCount());
        Assert.IsTrue(_dapper.AddStack("Stack 2"));
        Assert.AreEqual(2, GetStackCount());
        Assert.IsFalse(_dapper.AddStack("Stack 2"));
        
        var stack1 = _testConnection.QuerySingle<Stack>("SELECT * FROM Stacks WHERE Id = 1");
        Assert.AreEqual(1, stack1.Id);
        Assert.AreEqual("Stack 1", stack1.Name);
        
        var stack2 = _testConnection.QuerySingle<Stack>("SELECT * FROM Stacks WHERE Id = 2");
        Assert.AreEqual(2, stack2.Id);
        Assert.AreEqual("Stack 2", stack2.Name);
    }

    [TestMethod]
    public void DeleteStackTest()
    {
        _dapper.InitializeDatabase();
        _dapper.AddStack("Stack 1");
        _dapper.AddStack("Stack 2");
        _dapper.AddStack("Stack 3");

        var card1 = new Card() { Front = "Front1", Back = "Back1", StackId = 1 };
        var card2 = new Card() { Front = "Front2", Back = "Back2", StackId = 2 };
        var card3 = new Card() { Front = "Front3", Back = "Back3", StackId = 3 };
        Assert.IsTrue(_dapper.AddCard(card1.Front, card1.Back, "Stack 1"));
        Assert.IsTrue(_dapper.AddCard(card2.Front, card2.Back, "Stack 2"));
        Assert.IsTrue(_dapper.AddCard(card3.Front, card3.Back, "Stack 3"));
        Assert.AreEqual(3, GetCardCount());
        
        Assert.AreEqual(3, GetStackCount());
        Assert.IsTrue(_dapper.DeleteStack(2));
        Assert.AreEqual(2, GetStackCount());
        
        Assert.AreEqual(2, GetCardCount());
        var card = _testConnection.QueryFirst<Card>("SELECT * FROM Cards");
        Assert.AreEqual(card1.Front, card.Front);
        Assert.AreEqual(card1.Back, card.Back);
        Assert.AreEqual(card1.StackId, card.StackId);
        
        Assert.IsTrue(_dapper.DeleteCard(1));
        Assert.AreEqual(1, GetCardCount());
        card = _testConnection.QueryFirst<Card>("SELECT * FROM Cards");
        Assert.AreEqual(card3.Front, card.Front);
        Assert.AreEqual(card3.Back, card.Back);
        Assert.AreEqual(card3.StackId, card.StackId);
    }
    
    private bool IsDatabaseCreated()
    {
        const string sql = "SELECT DB_ID(N'FlashCardsTestDB') AS [DatabaseID];";
        var res = _testConnection.ExecuteScalar(sql);
        return (res is not null);
    }

    private bool IsTableCreated(string table)
    {
        const string sql = "SELECT COUNT(*) FROM sys.tables WHERE name = @table";
        return _testConnection.ExecuteScalar<int>(sql, new { table }) > 0;
    }

    private int GetStackCount()
    {
        const string sql = "SELECT COUNT(*) FROM Stacks";
        return _testConnection.ExecuteScalar<int>(sql);
    }

    private int GetCardCount()
    {
        const string sql = "SELECT COUNT(*) FROM Cards";
        return _testConnection.ExecuteScalar<int>(sql);
    }
}