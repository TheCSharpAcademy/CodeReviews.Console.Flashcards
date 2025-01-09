using Dapper;
using Flashcards.AshtonLeeSeloka.DTO;
using Flashcards.AshtonLeeSeloka.Models;
using FlashcardStack.AshtonLeeSeloka.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;
namespace FlashcardStack.AshtonLeeSeloka.Services;

internal class DataService
{
	private readonly string? _connection = ConfigurationManager.AppSettings.Get("ConnectionString");
	public List<StackModel> GetAvailableStacks()
	{
		SqlConnection connection = new SqlConnection(_connection);
		var sqlCommand = "SELECT * FROM stack";
		var stacks = connection.Query<Models.StackModel>(sqlCommand);
		List<StackModel> stackResults = new List<StackModel>();

		foreach (var stack in stacks)
		{
			stackResults.Add(new StackModel() { Stack_ID = stack.Stack_ID, Stack_Name = stack.Stack_Name });
		}
		return stackResults;
	}

	public List<CardDTO> GetCards(StackModel stack) 
	{
		SqlConnection connection = new SqlConnection(_connection);
		var SqlCommand = @"SELECT stack.Stack_Name,Cards.ID,Cards.Front, Cards.Back
						FROM stack
						INNER JOIN Cards 
						ON stack.Stack_ID = Cards.Stack_ID
						WHERE Cards.Stack_ID =@Stack_ID;";
		var Cards = connection.Query<CardDTO>(SqlCommand, new { Stack_ID = stack.Stack_ID});
		List<CardDTO> RetrievedCards = new List<CardDTO>();

		foreach (var card in Cards) 
		{
			RetrievedCards.Add(new CardDTO() {ID = card.ID, Stack_Name = card.Stack_Name, Front = card.Front, Back = card.Back });
		}
		return RetrievedCards;
	}

	public void InsertCard(string front, string back, int? foreignKey) 
	{
		SqlConnection connection = new SqlConnection(_connection);
		var sqlCommand = @"INSERT INTO Cards(Front, Back, Stack_ID) VALUES(@front, @back, @stack_ID);";
		connection.Execute(sqlCommand, new { front = front,back = back, stack_ID = foreignKey });
	}

	public void EditCard(string front, string back, int? ID) 
	{
		SqlConnection connection = new SqlConnection(_connection);
		var sqlCommand = @"UPDATE Cards
							SET Front = @front, Back = @back
							WHERE ID = @ID;";
		connection.Execute(sqlCommand, new {front = front,back = back, ID = ID });
	}

	public void DeleteCard(int? ID) 
	{
		SqlConnection connection = new SqlConnection(_connection);
		var sqlCommand = @"DELETE FROM Cards
							WHERE ID = @ID;";
		connection.Execute(sqlCommand, new { ID = ID });
	}

	public void DeleteStack(int? ID) 
	{
		SqlConnection connection = new SqlConnection(_connection);
		var sqlCommand = @"DELETE FROM stack
							WHERE Stack_ID = @ID;";
		connection.Execute(sqlCommand, new { ID = ID });
	}

	public void InsertNewStack(string stackName)
	{
		SqlConnection connection = new SqlConnection(_connection);
		var sqlCommand = @"INSERT INTO stack(Stack_Name) VALUES(@stackName);";
		connection.Execute(sqlCommand, new { stackName = stackName });
	}

	public int GetStackID(string stackName) 
	{
		SqlConnection connection = new SqlConnection(_connection);
		var sqlCommand = @"SELECT Stack_ID
							FROM stack
							WHERE Stack_Name = @stackName";
		var stack_ID = connection.QuerySingle<int>(sqlCommand, new {stackName = stackName });
		return stack_ID;
	}

	public void InsertScore(string stack, DateTime study_date ,int score, int stack_ID) 
	{
		SqlConnection connection = new SqlConnection(_connection);
		var sqlCommand = @"INSERT INTO study_session(Stack, Study_date, Score, Stack_ID) VALUES(@stack, @study_date, @score, @stack_ID)";
		connection.Execute(sqlCommand, new {stack = stack, study_date=study_date, score = score, stack_ID = stack_ID});
	}

	public List<Report> PivotDateAverageSCore(int? year, string stack)
	{
		SqlConnection connection = new SqlConnection(_connection);
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

		var values = connection.Query<Report>(sqlCommand, new { year= year, stack = stack });
		List<Report> results = new List<Report>();
		foreach (Report report in values)
		{
			results.Add(report);
		}
		return results;
	}

	public List<Report> PivotDateCountEntries(int? year, string stack)
	{
		SqlConnection connection = new SqlConnection(_connection);
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
		return results;
	}

	public List<int> GetAvailableStackYears(string stack) 
	{ 
		SqlConnection connection = new SqlConnection(_connection);
		var sqlCommand = @"SELECT YEAR(Study_date)
							FROM study_session
							WHERE CAST(Stack AS VARCHAR(100)) = @stack
							GROUP BY YEAR(Study_date);";
		var values = connection.Query<int>(sqlCommand, new {stack = stack});
		return values.ToList();
	}
}
