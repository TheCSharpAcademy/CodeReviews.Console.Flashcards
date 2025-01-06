using Dapper;
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
}
