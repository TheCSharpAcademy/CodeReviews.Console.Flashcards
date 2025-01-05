namespace Flashcards.AshtonLeeSeloka.Services;
using Microsoft.Data.SqlClient;
using System.Configuration;

internal class DataService
{
	private readonly string? _ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
	private readonly string? _DBCreationString = ConfigurationManager.AppSettings.Get("DBCreationString");

	public void createDB()
	{
		try
		{
			using (SqlConnection connection = new SqlConnection(_DBCreationString))
			{
				connection.Open();
				
				var SQLCommand = connection.CreateCommand();
				SQLCommand.CommandText = @"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name ='FlashCardsDB')
															BEGIN
																CREATE DATABASE FlashCardsDB;
															END;";
				SQLCommand.ExecuteNonQuery();
				Console.WriteLine("Sucess)");
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}

	}
}
