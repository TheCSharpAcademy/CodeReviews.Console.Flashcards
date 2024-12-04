

using Dapper;
using Flashcards.Bina28.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;



namespace Flashcards.Bina28.DBmanager;
internal class StacksDB
{
	private string connectionString;
	public StacksDB()
	{

		connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
	}

	public List<StacksDto> GetAllRecords()
	{
		var stacks = new List<StacksDto>();

		try
		{
			using var connection = new SqlConnection(connectionString);
			connection.Open();

			string sql = "SELECT name FROM stacks";
			return connection.Query<StacksDto>(sql).ToList();
		}
		catch (SqlException ex)
		{

			Console.WriteLine("SQL Error: " + ex.Message);
		}
		catch (Exception ex)
		{

			Console.WriteLine("Error: " + ex.Message);
		}
		return stacks;
	}
	internal bool CreateStack(string? stackName)
	{
		string query = "INSERT INTO stacks (name) VALUES (@stackName)";
		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@stackName", stackName);
			try
			{
				connection.Open();
				int result = command.ExecuteNonQuery();
				return result > 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
				return false;
			}
		}
	}


	internal bool DeleteStack(string? stackName)
	{
		if (string.IsNullOrWhiteSpace(stackName))
		{
			Console.WriteLine("Stack name cannot be empty.");
			return false;
		}

		string selectQuery = "SELECT stack_id FROM stacks WHERE name = @stackName";
		string deleteQuery = "DELETE FROM stacks WHERE stack_id = @stack_id";

		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();

				// First, retrieve the stack_id
				SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
				selectCommand.Parameters.AddWithValue("@stackName", stackName);
				object? stack_id = selectCommand.ExecuteScalar();

				if (stack_id == null)
				{
					Console.WriteLine("Stack not found.");
					return false;
				}

				// Now, delete the stack using stack_id
				SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
				deleteCommand.Parameters.AddWithValue("@stack_id", stack_id);

				int result = deleteCommand.ExecuteNonQuery();
				return result > 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
				return false;
			}
		}

	}

	internal bool UpdateStack(string stackName, string newStackName)
	{
		if (string.IsNullOrWhiteSpace(stackName) || string.IsNullOrWhiteSpace(newStackName))
		{
			Console.WriteLine("Stack name cannot be empty.");
			return false;
		}

		if (stackName == newStackName)
		{
			Console.WriteLine("New stack name cannot be the same as the old stack name.");
			return false;
		}

		string query = "UPDATE stacks SET name = @newStackName WHERE name = @stackName";
		using (var connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();
				int rowsAffected = connection.Execute(query, new { stackName, newStackName });
				if (rowsAffected > 0)
				{
					Console.WriteLine("Stack name updated successfully.");
					return true;
				}
				else
				{
					Console.WriteLine("Stack not found or no changes made.");
					return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
				return false;
			}
		}
	}

	// Method to create the Stacks table
	public void CreateStacksTable()
	{
		string createTableQuery = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks')
        BEGIN
            CREATE TABLE Stacks (
                stack_id INT IDENTITY(1,1) PRIMARY KEY,  
                name NVARCHAR(100) NOT NULL
            );
        END";

		// Execute the query to create the table
		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();

				using (SqlCommand command = new SqlCommand(createTableQuery, connection))
				{
					command.ExecuteNonQuery();

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred: " + ex.Message);

			}
		}
	}

	// Method to insert stacks into the table
	internal void InsertStacks()
	{
		string query = "INSERT INTO stacks (name) VALUES (@name)";

		var stacks = new List<StacksDto>
	{
		new StacksDto("Norwegian"),
		new StacksDto("English"),
		new StacksDto("Spanish"),
		new StacksDto("German"),

	};

		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();
				// Reset the identity seed for stacks table
				string resetStackIdentityQuery = "DBCC CHECKIDENT ('stacks', RESEED, 0);";
				using (SqlCommand resetStackCommand = new SqlCommand(resetStackIdentityQuery, connection))
				{
					resetStackCommand.ExecuteNonQuery();
				}
				foreach (var stack in stacks)
				{
					// Check if stack already exists
					string checkIfExistsQuery = "SELECT COUNT(1) FROM stacks WHERE name = @name";
					using (SqlCommand checkCommand = new SqlCommand(checkIfExistsQuery, connection))
					{
						checkCommand.Parameters.AddWithValue("@name", stack.Name);
						int count = (int)checkCommand.ExecuteScalar();

						if (count == 0) // If stack doesn't exist, insert it
						{
							using (SqlCommand command = new SqlCommand(query, connection))
							{
								command.Parameters.AddWithValue("@name", stack.Name);
								command.ExecuteNonQuery();


							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while inserting stacks: {ex.Message}");

			}
		}
	}

}






