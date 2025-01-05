namespace Flashcards.AshtonLeeSeloka.Services;
using Microsoft.Data.SqlClient;
using System.Configuration;

internal class DataService
{
	private readonly string? _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
	

	public void createDB()
	{
		try
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				Console.WriteLine("Sucess)");
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}

	}
}
