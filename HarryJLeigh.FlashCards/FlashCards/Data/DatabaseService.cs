using Dapper;
using System.Configuration;
using System.Data.SqlClient;

namespace FlashCards.Data;

public class DatabaseService
{
    private readonly string DatabasePath;

    public DatabaseService()
    {
        var connectionStringSettings = ConfigurationManager.ConnectionStrings["DatabasePath"];
        if (connectionStringSettings== null)
        {
            throw new Exception("DatabasePath connection string is not configured.");
        }
        DatabasePath = connectionStringSettings.ConnectionString;
        using var connection = GetConnection();
        CreateTables(connection);
    }

    internal SqlConnection GetConnection() => new SqlConnection(DatabasePath);
    
    private void CreateTables(SqlConnection connection)
    {
        var createStacksTable = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Stacks' AND xtype='U')
                                      CREATE TABLE Stacks (
                                          id INT IDENTITY(1,1) PRIMARY KEY,
                                          name VARCHAR(100) NOT NULL
                                      );";
        
        var createFlashCardsTable = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FlashCards' AND xtype='U')
                                          CREATE TABLE FlashCards (
                                              flashCardId INT IDENTITY(1,1) PRIMARY KEY,
                                              front VARCHAR(100) NOT NULL,
                                              back VARCHAR(100) NOT NULL,
                                              stack_id INT,
                                              FOREIGN KEY (stack_id) REFERENCES Stacks(id)
                                          );";
        
        
        var createStudyTable = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Study' AND xtype='U')
                                 CREATE TABLE Study (
                                     id INT IDENTITY(1,1) PRIMARY KEY,
                                     studyDate VARCHAR(100) NOT NULL,
                                     score INT NOT NULL,
                                     flashcard_amount INT NOT NULL,
                                     stack_id INT,
                                     FOREIGN KEY (stack_id) REFERENCES Stacks(id) ON DELETE CASCADE
                                 );";
        
        connection.Open();
        connection.Execute(createStacksTable);
        connection.Execute(createFlashCardsTable);
        connection.Execute(createStudyTable);
    }
}