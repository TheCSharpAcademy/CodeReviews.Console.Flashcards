using Flashcards.Arashi256.Config;
using Dapper;
using System.Data.SqlClient;

namespace Flashcards.Arashi256.Classes
{
    internal class Database
    {
        private bool hasError = false;
        private DatabaseConnection _connection;

        public Database()
        {
            _connection = new DatabaseConnection();
        }

        public DatabaseConnection GetConnection()
        {
            return _connection;
        }

        public bool TestConnection()
        {
            if (CheckTables(false))
            {
                if (!hasError)
                {
                    Console.WriteLine("Database initialised and ready.");
                }
                else
                {
                    HaltError();
                }
            }
            else
            {
                if (!hasError)
                {
                    Console.WriteLine("Database already present and correct.");
                }
                else
                {
                    HaltError();
                }
            }
            return true;
        }

        private void HaltError()
        {
            Console.Clear();
            Console.WriteLine("STOP ERROR: Application cannot continue operation.");
            Console.WriteLine("Likely causes:-");
            Console.WriteLine("- Database specified in the connection string doesn't exist");
            Console.WriteLine("- Connection string user credentials are wrong");
            Console.WriteLine("- User credentials are correct but the user permissions are insufficient to create the application tables");
            Console.WriteLine("\nPlease check your connection string details in the App.config file!\n");
            Environment.Exit(0);
        }

        public int TableExists(string tableName)
        {
            int result = 0;
            string sql = @"SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = @TableName";
            var parameters = new DynamicParameters();
            parameters.Add("@TableName", tableName);
            result = ExecuteScalarQuery(sql, parameters);
            return result;
        }

        public int ExecuteQuery(string query, DynamicParameters? parameters = null)
        {
            int result = 0;
            hasError = false;
            using (var connection = new SqlConnection(_connection.DatabaseConnectionString))
            {
                try
                {
                    result = connection.Execute(query, parameters);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GENERAL ERROR: " + ex.Message);
                    hasError = true;
                }
            }
            return result;
        }

        private int ExecuteScalarQuery(string query, DynamicParameters? parameters = null)
        {
            int result = 0;
            hasError = false;
            using (var connection = new SqlConnection(_connection.DatabaseConnectionString))
            {
                try
                {
                    result = connection.ExecuteScalar<int>(query, parameters);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GENERAL ERROR: " + ex.Message);
                    hasError = true; 
                }
            }
            return result;
        }

        public bool CheckTables(bool force)
        {
            int creationCount = 0;
            creationCount += TableExists("stacks");
            creationCount += TableExists("flashcards");
            creationCount += TableExists("studysessions");
            if (creationCount < 3 || force)
            {
                ExecuteQuery("IF OBJECT_ID('dbo.flashcards', 'U') IS NOT NULL DROP TABLE dbo.flashcards;");
                ExecuteQuery("IF OBJECT_ID('dbo.stacks', 'U') IS NOT NULL DROP TABLE dbo.stacks;");
                ExecuteQuery("IF OBJECT_ID('dbo.studysessions', 'U') IS NOT NULL DROP TABLE dbo.studysession;");
                ExecuteQuery(@"CREATE TABLE dbo.stacks (Id INT IDENTITY(1,1) PRIMARY KEY, Subject NVARCHAR(100) NOT NULL);");
                ExecuteQuery(@"CREATE TABLE dbo.flashcards (
                                    Id INT IDENTITY(1,1) PRIMARY KEY,
                                    StackId INT NOT NULL,
                                    Front NVARCHAR(255) NOT NULL,
                                    Back NVARCHAR(255) NOT NULL,
                                    CONSTRAINT FK_Flashcards_Stack FOREIGN KEY (StackId)
                                    REFERENCES dbo.stacks (Id)
                                    ON DELETE CASCADE);");           
                ExecuteQuery(@"CREATE TABLE dbo.studysessions (
                                    Id INT IDENTITY(1,1) PRIMARY KEY, 
                                    StackId INT NOT NULL,
                                    TotalCards INT NOT NULL, 
                                    Score INT NOT NULL,
                                    DateStudied DATE NOT NULL,
                                    CONSTRAINT FK_StudySession_Stack FOREIGN KEY (StackId)
                                    REFERENCES dbo.stacks (Id)
                                    ON DELETE CASCADE);");
                return true;
            }
            return false;
        }
    }
}