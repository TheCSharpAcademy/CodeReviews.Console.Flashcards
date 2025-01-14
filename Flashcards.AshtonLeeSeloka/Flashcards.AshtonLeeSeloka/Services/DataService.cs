using Dapper;
using Flashcards.AshtonLeeSeloka.DTO;
using Flashcards.AshtonLeeSeloka.Models;
using FlashcardStack.AshtonLeeSeloka.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;
namespace FlashcardStack.AshtonLeeSeloka.Services;

internal class DataService
{
	private readonly string? _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
	public List<StackModel> GetAvailableStacks()
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var sqlCommand = "SELECT * FROM stack";
		var stacks = connection.Query<Models.StackModel>(sqlCommand);
		List<StackModel> stackResults = new List<StackModel>();

		foreach (var stack in stacks)
		{
			stackResults.Add(new StackModel() { StackID = stack.StackID, StackName = stack.StackName });
		}
		connection.Dispose();
		return stackResults;
	}

	public List<Card> GetCards(StackModel stack)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var SqlCommand = @"SELECT stack.StackName,Cards.ID,Cards.Front, Cards.Back
						FROM stack
						INNER JOIN Cards 
						ON stack.StackID = Cards.StackID
						WHERE Cards.StackID =@StackID;";
		var Cards = connection.Query<Card>(SqlCommand, new { StackID = stack.StackID });
		connection.Dispose();
		List<Card>? RetrievedCards = new List<Card>();

		foreach (var card in Cards)
		{
			RetrievedCards.Add(new Card() { ID = card.ID, StackName = card.StackName, Front = card.Front, Back = card.Back });
		}
		return RetrievedCards;
	}

	public void InsertCard(string front, string back, int? foreignKey)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var sqlCommand = @"INSERT INTO Cards(Front, Back, StackID) VALUES(@front, @back, @stack_ID);";
		connection.Execute(sqlCommand, new { front = front, back = back, stack_ID = foreignKey });
		connection.Dispose();
	}

	public void EditCard(string front, string back, int? ID)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var sqlCommand = @"UPDATE Cards
							SET Front = @front, Back = @back
							WHERE ID = @ID;";
		connection.Execute(sqlCommand, new { front = front, back = back, ID = ID });
		connection.Dispose();
	}

	public void DeleteCard(int? ID)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var sqlCommand = @"DELETE FROM Cards
							WHERE ID = @ID;";
		connection.Execute(sqlCommand, new { ID = ID });
		connection.Dispose();
	}

	public void DeleteStack(int? ID)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var sqlCommand = @"DELETE FROM stack
							WHERE StackID = @ID;";
		connection.Execute(sqlCommand, new { ID = ID });
		connection.Dispose();
	}

	public void InsertNewStack(string stackName)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var sqlCommand = @"INSERT INTO stack(StackName) VALUES(@stackName);";
		connection.Execute(sqlCommand, new { stackName = stackName });
		connection.Dispose();
	}

	public int GetStackID(string stackName)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var sqlCommand = @"SELECT StackID
							FROM stack
							WHERE StackName = @stackName";
		var stack_ID = connection.QuerySingle<int>(sqlCommand, new { stackName = stackName });
		connection.Dispose();
		return stack_ID;
	}

	public void InsertScore(string stack, DateTime study_date, int score, int stack_ID)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var sqlCommand = @"INSERT INTO study_session(Stack, Study_date, Score, StackID) VALUES(@stack, @study_date, @score, @stack_ID)";
		connection.Execute(sqlCommand, new { stack = stack, study_date = study_date, score = score, stack_ID = stack_ID });
		connection.Dispose();
	}

	public List<Report> PivotDateAverageSCore(int? year, string stack)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var sqlCommand = @"SELECT 
						CAST(stack AS VARCHAR(255)) AS stack,  
						AVG(CASE WHEN Months = 1 THEN Average_Score ELSE 0 END) AS Jan,
						AVG(CASE WHEN Months = 2 THEN Average_Score ELSE 0 END) AS Feb,
						AVG(CASE WHEN Months = 3 THEN Average_Score ELSE 0 END) AS Mar,
						AVG(CASE WHEN Months = 4 THEN Average_Score ELSE 0 END) AS Apr,
						AVG(CASE WHEN Months = 5 THEN Average_Score ELSE 0 END) AS May,
						AVG(CASE WHEN Months = 6 THEN Average_Score ELSE 0 END) AS Jun,
						AVG(CASE WHEN Months = 7 THEN Average_Score ELSE 0 END) AS Jul,
						AVG(CASE WHEN Months = 8 THEN Average_Score ELSE 0 END) AS Aug,
						AVG(CASE WHEN Months = 9 THEN Average_Score ELSE 0 END) AS Sep,
						AVG(CASE WHEN Months = 10 THEN Average_Score ELSE 0 END) AS Oct,
						AVG(CASE WHEN Months = 11 THEN Average_Score ELSE 0 END) AS Nov,
						AVG(CASE WHEN Months = 12 THEN Average_Score ELSE 0 END) AS Dec
					FROM (
						SELECT  
							CAST(Stack AS VARCHAR(255)) AS stack,  
							AVG(study_session.Score) AS Average_Score,
							MONTH(study_session.Study_date) AS Months
						FROM study_session
						WHERE YEAR(study_session.Study_date) = @year AND CAST(Stack AS VARCHAR(100))=@stack
						GROUP BY MONTH(study_session.Study_date), CAST(Stack AS VARCHAR(255))  
					) AS sub
					GROUP BY stack;";

		var values = connection.Query<Report>(sqlCommand, new { year = year, stack = stack });
		List<Report> results = new List<Report>();
		foreach (Report report in values)
		{
			results.Add(report);
		}
		connection.Dispose();
		return results;
	}

	public List<Report> PivotDateCountEntries(int? year, string stack)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var sqlCommand = @"SELECT 
						CAST(stack AS VARCHAR(255)) AS stack,
						COUNT(CASE WHEN Months = 1 THEN 1 END) AS Jan,
						COUNT(CASE WHEN Months = 2 THEN 1 END) AS Feb,
						COUNT(CASE WHEN Months = 3 THEN 1 END) AS Mar,
						COUNT(CASE WHEN Months = 4 THEN 1 END) AS Apr,
						COUNT(CASE WHEN Months = 5 THEN 1 END) AS May,
						COUNT(CASE WHEN Months = 6 THEN 1 END) AS Jun,
						COUNT(CASE WHEN Months = 7 THEN 1 END) AS Jul,
						COUNT(CASE WHEN Months = 8 THEN 1 END) AS Aug,
						COUNT(CASE WHEN Months = 9 THEN 1 END) AS Sep,
						COUNT(CASE WHEN Months = 10 THEN 1 END) AS Oct,
						COUNT(CASE WHEN Months = 11 THEN 1 END) AS Nov,
						COUNT(CASE WHEN Months = 12 THEN 1 END) AS Dec
					FROM (
						SELECT  
							CAST(Stack AS VARCHAR(100)) AS stack,
							MONTH(study_session.Study_date) AS Months
						FROM study_session
						WHERE YEAR(study_session.Study_date) = @year AND CAST(Stack AS VARCHAR(100))=@stack
					) AS sub
					GROUP BY stack;
					";

		var values = connection.Query<Report>(sqlCommand, new { year = year, stack = stack });
		List<Report> results = new List<Report>();
		foreach (Report report in values)
		{
			results.Add(report);
		}
		connection.Dispose();
		return results;
	}

	public List<int> GetAvailableStackYears(string stack)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		var sqlCommand = @"SELECT YEAR(Study_date)
							FROM study_session
							WHERE CAST(Stack AS VARCHAR(100)) = @stack
							GROUP BY YEAR(Study_date);";
		var values = connection.Query<int>(sqlCommand, new { stack = stack });
		connection.Dispose();
		return values.ToList();
	}
}
