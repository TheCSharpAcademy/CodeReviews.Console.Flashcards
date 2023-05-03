using System.Configuration;
using System.Data.SqlClient;

namespace LucianoNicolasArrieta.Flashcards.Persistence
{
    public class DBManager
    {
        private string pathDB = ConfigurationManager.AppSettings.Get("DbFilePath");
        private string DBName = ConfigurationManager.AppSettings.Get("DbName");
        private string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        public void DBInit()
        {
            CreateDB();
            CreateTables();
        }

        private void CreateTables()
        {
            string strStacksTable = $@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Stack' and xtype='U')
                                        CREATE TABLE Stack (
                                            Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                            Subject varchar(100) NOT NULL
                                        )";

            string strFlashcardsTable = $@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Flashcard' and xtype='U')
                                            CREATE TABLE Flashcard (
                                                Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                                StackId INT NOT NULL,
                                                Question varchar(100) NOT NULL,
                                                Answer varchar(100) NOT NULL,
                                                FOREIGN KEY (StackId) REFERENCES Stack(Id)
                                            )";

            string strSessionsTable = $@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StudySession' and xtype='U')
                                            CREATE TABLE StudySession (
                                                Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                                StackId INT NOT NULL,
                                                SessionDate DATE NOT NULL,
                                                Questions int NOT NULL,
                                                CorrectAnswers int NOT NULL,
                                                FOREIGN KEY (StackId) REFERENCES Stack(Id)
                                            )";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd1 = new SqlCommand(strStacksTable, connection);
                SqlCommand cmd2 = new SqlCommand(strFlashcardsTable, connection);
                SqlCommand cmd3 = new SqlCommand(strSessionsTable, connection);

                connection.Open();
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
            }
        }

        private void CreateDB()
        {
            string str =
                @$"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{DBName}')
                BEGIN
                    CREATE DATABASE {DBName} ON PRIMARY
                    (NAME = {DBName}, 
                    FILENAME = '{pathDB}{DBName}.mdf',
                    SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)
                    LOG ON (NAME = {DBName}_Log,
                    FILENAME = '{pathDB}{DBName}.ldf',
                    SIZE = 1MB,
                    MAXSIZE = 5MB,
                    FILEGROWTH = 10%)
                END";

            SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder(connectionString)
            { InitialCatalog = "master" };
            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            {
                SqlCommand myCommand = new SqlCommand(str, connection);
                
                connection.Open();
                myCommand.ExecuteNonQuery();
            }
        }
    }
}
