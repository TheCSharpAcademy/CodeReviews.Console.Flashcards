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
		var SqlCommand = @"SELECT stack.Stack_Name,Cards.Front, Cards.Back
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
		var sqlCommand = @"INSERT INTO Cards(Front, Back, Stack_ID) VALUES(@front, @back, @stack_ID)";
		connection.Execute(sqlCommand, new { front = front,back = back, stack_ID = foreignKey });
	}
}
