namespace FlashcardStack.AshtonLeeSeloka.Services;
using Dapper;
using Flashcards.AshtonLeeSeloka.Services;
using Microsoft.Data.SqlClient;
using System.Configuration;

internal class StartUpDataService
{
	private readonly string? _ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
	private readonly string? _DBCreationString = ConfigurationManager.AppSettings.Get("DBCreationString");
	private readonly ValidationService _validationService = new ValidationService();

	public void StartUpDB()
	{
		CreateDB();
		CreateTables();
		SeedTableStack();
		SeedTablesCards();
	}

	public void CreateDB()
	{
		string dBName = _validationService.GetDBName(_ConnectionString);
		string sql = $@"
        IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{dBName}')
        BEGIN
            CREATE DATABASE [{dBName}];
        END;";

		try
		{
			using (SqlConnection connection = new SqlConnection(_DBCreationString))
			{
				connection.Open();
				SqlCommand s = new SqlCommand(sql, connection);
				s.Parameters.AddWithValue("db", dBName);
				s.ExecuteNonQuery();
				connection.Close();
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
	}

	public void CreateTables()
	{
		var connection = new SqlConnection(_ConnectionString);
		var SQLCommandStack = @"IF OBJECT_ID(N'dbo.stack',N'U') IS NULL
							CREATE TABLE stack(
							StackID INT IDENTITY(1,1) PRIMARY KEY,
							StackName VARCHAR(20) NOT NULL UNIQUE,
							);";
		connection.Execute(SQLCommandStack);

		var SQLCommandCard = @"IF OBJECT_ID(N'dbo.cards',N'U') IS NULL
							CREATE TABLE dbo.Cards (
							ID INT IDENTITY(1,1) PRIMARY KEY,
							Front TEXT,
							Back TEXT,
							StackID int Not Null,
							FOREIGN KEY (StackID) REFERENCES stack (StackID) ON DELETE CASCADE
							);";
		connection.Execute(SQLCommandCard);

		var SQLCommandStudy = @"IF OBJECT_ID(N'dbo.study_session',N'U') IS NULL
							CREATE TABLE study_session(
							ID INT IDENTITY(1,1) PRIMARY KEY,
							Stack TEXT NOT NULL,
							Study_date DATE,
							Score DECIMAL(3,1),
							StackID INT NOT NULL,
							FOREIGN KEY (StackID) REFERENCES stack (StackID) ON DELETE CASCADE
							);";
		connection.Execute(SQLCommandStudy);
		connection.Dispose();
	}

	public void SeedTableStack()
	{
		var connection = new SqlConnection(_ConnectionString);
		var SQLCommand = @" IF EXISTS (SELECT * FROM stack)
								SELECT 'TABLE IS NOT EMPTY'
								ELSE				
								INSERT INTO stack(StackName) VALUES('German'),('Spanish'),('Swahili');";
		connection.Execute(SQLCommand);
		connection.Dispose();
	}

	public void SeedTablesCards()
	{
		var connection = new SqlConnection(_ConnectionString);
		var SQLCommand = @" IF EXISTS (SELECT * FROM dbo.Cards)
							SELECT 'TABLE IS NOT EMPTY'
							ELSE
							INSERT INTO Cards(Front,Back,StackID)
							VALUES('How do you say Thank you in German?','Danke',1),
							('What is the correct form of the verb in the sentence: He ____ to the store','geht',1),
							('How do you ask someones name in German?','Wie heißen Sie?',1),
							('Translate to German: I am tired.','Ich bin müde.',1),
							('What is the plural form of ""Kind"" (child) in German','Kinder',1),
							('How do you say Good morning in Spanish?','Buenos días',2),
							('Translate to Spanish: I am learning Spanish.','Estoy aprendiendo español',2),
							('What is the Spanish word for ""house""','Casa',2),
							('What is the correct form of the verb ""to be"" for ""they"" in the present tense in Spanish?','Son',2),
							('How do you ask ""What time is it?"" in Spanish?','¿Qué hora es?',2),
							('How do you say ""Thank you"" in Swahili?','Asante',3),
							('Translate to Swahili: ""I am happy.""','Mimi ni furaha',3),
							('What is the Swahili word for ""book""?','Kitabu',3),
							('What is the Swahili word for ""friend""? ','Rafiki',3),
							('How do you ask ""Where are you from?"" in Swahili?','Unatoka wapi?',3);";
		connection.Execute(SQLCommand);
		connection.Dispose();
	}
}
