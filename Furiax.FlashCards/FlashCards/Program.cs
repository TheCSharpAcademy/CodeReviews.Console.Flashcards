using System.Data.SqlClient;
using System.Configuration;
using FlashCards;

var connectionString = ConfigurationManager.ConnectionStrings["Flashcards"].ConnectionString;

UserInput.ShowMenu();

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
