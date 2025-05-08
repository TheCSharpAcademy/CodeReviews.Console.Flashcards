using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace STUDY.ConsoleApp.Flashcards;

public class DatabaseHelper
{
    private readonly IConfiguration _configuration;

    public DatabaseHelper()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
	        .Build();
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

    public void CreateTables()
    {
        using var connection = GetConnection();
        var sqlCommand = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'stack')
						BEGIN
							CREATE TABLE stack (
								stack_id INT PRIMARY KEY IDENTITY(1,1),
								stack_name NVARCHAR(50) UNIQUE NOT NULL
							);
						END

						IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'flashcards')
						BEGIN
						Create Table flashcards (
								card_id INT PRIMARY KEY IDENTITY(1,1),
								card_front NVARCHAR(100) NOT NULL,
								card_back NVARCHAR(100) NOT NULL,
								stack_id INT NOT NULL,
								FOREIGN KEY (stack_id) REFERENCES stack(stack_id) ON DELETE CASCADE

							);
						END

						IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'study_sessions')
						BEGIN
							CREATE TABLE study_sessions (
								session_id INT PRIMARY KEY IDENTITY(1,1),
								session_date DATETIME NOT NULL,
								stack_id INT NOT NULL,
								score INT
								FOREIGN KEY (stack_id) REFERENCES stack(stack_id) ON DELETE CASCADE
							);
						END";
        connection.Execute(sqlCommand);
    }
}