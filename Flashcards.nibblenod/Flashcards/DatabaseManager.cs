using Dapper;
using Microsoft.Data.SqlClient;

namespace Flashcards
{
    internal class DatabaseManager
    {
        string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");
        internal string connectionStringWithDB = System.Configuration.ConfigurationManager.AppSettings.Get("connectionStringWithDB");
        internal void CreateDb(string dbName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string command = @$"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{dbName}')
                    BEGIN 
                        CREATE DATABASE {dbName}
                    END
                                    ";


                connection.Execute(command);

            }
        }

        internal void CreateTables()
        {
            using (var connection = new SqlConnection(connectionStringWithDB))
            {
                string command = $@"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                               BEGIN
                                    CREATE TABLE Stacks (
                                    ID INT IDENTITY(1,1) PRIMARY KEY,
                                    NAME VARCHAR(100) UNIQUE NOT NULL);
                                END

                                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                                BEGIN 
                                    CREATE TABLE Flashcards (
                                    ID INT IDENTITY(1,1) PRIMARY KEY,
                                    Front VARCHAR(100),
                                    Back VARCHAR(100),
                                    StackID INT NOT NULL,
                                    FOREIGN KEY (StackID) REFERENCES Stacks(ID) ON DELETE CASCADE
                                    );
                                END 

                                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions')
                                BEGIN
                                    CREATE TABLE StudySessions (
                                    ID INT IDENTITY(1,1) PRIMARY KEY,
                                    SessionDate DATE,
                                    StackName VARCHAR(100) NOT NULL,
                                    Score INT,
                                    StackID INT NOT NULL,
                                    FOREIGN KEY (StackID) REFERENCES Stacks(ID) ON DELETE CASCADE
                                );
                                END
                                ";

                connection.Execute(command);

            }
        }
    }
}
