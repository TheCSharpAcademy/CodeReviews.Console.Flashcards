

using Microsoft.Data.SqlClient;



namespace Flashcards.Bina28.DBmanager;
internal class DBConfig
{
	public static string ConnectionString =System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

	public static SqlConnection EstablishConnection()
	{
		SqlConnection sqlConnection = new SqlConnection(ConnectionString);
		try
		{
			sqlConnection.Open();

			return sqlConnection; // Return the open connection
		}
		catch (Exception ex)
		{
			Console.WriteLine("Error: " + ex.Message);
			return null; // Return null if the connection fails
		}
	}
}
