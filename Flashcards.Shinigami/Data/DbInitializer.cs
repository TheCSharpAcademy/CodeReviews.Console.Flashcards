using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Flashcards.Data
{
    public static class DbInitializer
    {
        private static readonly string? _connectionString;

        static DbInitializer()
        {
            _connectionString = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("defaultConnection");
        }

        public static void CreateDefaultTables()
        {
            var sqlQueryStacks =
                @"
                IF NOT EXISTS(SELECT * FROM sysobjects WHERE name = 'Stacks' AND xtype = 'U')
                BEGIN
                 CREATE TABLE Stacks (
                  Id INT IDENTITY PRIMARY KEY,
                  Name NVARCHAR(100) NOT NULL UNIQUE
                 );
                END
                ";

            var sqlQueryFlashCards =
                @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Flashcards' AND xtype = 'U')
                BEGIN
                CREATE TABLE Flashcards (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Question NVARCHAR(255) NOT NULL,
                Answer NVARCHAR(255) NOT NULL,
                StackId INT NOT NULL,
                FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
                )
                END
                ";

            var sqlQueryStudySessions =
                @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'StudySessions' AND xtype = 'U')
                BEGIN
                CREATE TABLE StudySessions (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                StackId INT NOT NULL,
                StudyDate DATETIME NOT NULL,
                Score INT NOT NULL,
                FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
                )
                END
                ";

            IDbConnection dbConnection = new SqlConnection(_connectionString);
            dbConnection.Open();
            dbConnection.Execute(sqlQueryStacks);
            dbConnection.Execute(sqlQueryFlashCards);
            dbConnection.Execute(sqlQueryStudySessions);
            dbConnection.Close();
            dbConnection.Dispose();
        }
    }
}
