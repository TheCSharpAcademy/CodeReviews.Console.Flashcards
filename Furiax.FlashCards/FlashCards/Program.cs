using System.Data.SqlClient;
using System.Configuration;

string connectionString = ConfigurationManager.ConnectionStrings["Flashcards"].ConnectionString;

using (SqlConnection connection = new SqlConnection(connectionString))
{
	try
	{
		connection.Open();
		if (connection.State == System.Data.ConnectionState.Open)
		{
            Console.WriteLine("connection succesfull");
        }
		else
		{ Console.WriteLine("connection failed"); }
	}
	catch (Exception ex)
	{
        Console.WriteLine("An error occured: " + ex);
    }
}
