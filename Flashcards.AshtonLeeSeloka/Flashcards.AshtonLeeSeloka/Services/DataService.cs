using Dapper;
using Flashcards.AshtonLeeSeloka.DTO;
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

	public List<string> PivotDateAverageSCore(int year, string stack)
	{
		SqlConnection connection = new SqlConnection(_connection);
		var sqlCommand = @"SELECT 
						Stack,  
						ISNULL([1], 0) AS 'January',
						ISNULL([2], 0) AS 'February',
						ISNULL([3], 0) AS 'March',
						ISNULL([4], 0) AS 'April',
						ISNULL([5], 0) AS 'May',
						ISNULL([6], 0) AS 'June',
						ISNULL([7], 0) AS 'July',
						ISNULL([8], 0) AS 'August',
						ISNULL([9], 0) AS 'September',
						ISNULL([10], 0) AS 'October',
						ISNULL([11], 0) AS 'November',
						ISNULL([12], 0) AS 'December'
					FROM 
						(
							SELECT 
								CAST(Stack AS VARCHAR(MAX)) AS Stack,  
								MONTH(Study_Date) AS Month, 
								Score
							FROM 
								study_session
							WHERE YEAR(Study_date) = @year
						) AS SourceTable
					PIVOT (
						SUM(Score)  
						FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12]) 
					) AS PivotTable
					ORDER BY 
						Stack;";

		var values = connection.Query<IEnumerable<string>>(sqlCommand, new { year= year, stack = stack });
		List<string> result = new List<string>();

		return result;
	}
}
