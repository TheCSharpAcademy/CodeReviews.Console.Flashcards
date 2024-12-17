

using Dapper;
using Flashcards.Bina28.Models;
using Microsoft.Data.SqlClient;




namespace Flashcards.Bina28.DBmanager;
internal class StacksDB
{
	private string connectionString;
	public StacksDB()
	{
		connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
	}

	public List<StacksDto> GetAllRecords()
	{
		var stacks = new List<StacksDto>();

		try
		{
			using var connection = new SqlConnection(connectionString);
			connection.Open();

			string sql = "SELECT  name FROM stacks";
			stacks = connection.Query<StacksDto>(sql).ToList();
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
	
	public void CreateStacksTable()
	{
		string createTableQuery = @"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks')
BEGIN
    CREATE TABLE Stacks (
        stack_id INT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(255) UNIQUE NOT NULL,

    );
END;
";

		// Query to find the highest stack_id
		string findMaxStackIdQuery = "SELECT ISNULL(MAX(stack_id), 0) FROM Stacks";

		// Query to reseed the identity column
		string reseedQuery = "DBCC CHECKIDENT ('Stacks', RESEED, @maxStackId)";

		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();

				// Create the table if it doesn't exist
				using (SqlCommand createCommand = new SqlCommand(createTableQuery, connection))
				{
					createCommand.ExecuteNonQuery();					
				}

				// Find the highest stack_id
				int maxStackId;
				using (SqlCommand findMaxStackIdCommand = new SqlCommand(findMaxStackIdQuery, connection))
				{
					maxStackId = (int)findMaxStackIdCommand.ExecuteScalar();
				}

				// Reseed the identity column to avoid issues with the stack_id
				using (SqlCommand reseedCommand = new SqlCommand(reseedQuery, connection))
				{
					// Add the @maxStackId parameter
					reseedCommand.Parameters.AddWithValue("@maxStackId", maxStackId);
					reseedCommand.ExecuteNonQuery();					
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine("SQL Error: " + ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine("An unexpected error occurred: " + ex.Message);
			}
		}
	}

	
	public void CreateStack(string? stackName)
	{
		if (string.IsNullOrWhiteSpace(stackName))
		{
			Console.WriteLine("Stack name cannot be empty.");
			
		}

		string checkQuery = "SELECT COUNT(1) FROM stacks WHERE name = @stackName";

		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();

				using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
				{
					checkCommand.Parameters.AddWithValue("@stackName", stackName);
					int count = (int)checkCommand.ExecuteScalar();

					if (count > 0)
					{
						Console.WriteLine($"Stack '{stackName}' already exists.");						
					}
				}

				string query = "INSERT INTO stacks (name) VALUES (@stackName)";
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@stackName", stackName);
				int result = command.ExecuteNonQuery();

				if (result > 0)
				{
					Console.WriteLine($"The stack '{stackName}' was successfully created");					
				}
				else
				{
					Console.WriteLine("Failed to create the stack.");				
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");				
			}
		}
	}


	public void  DeleteStack(string? stackName)
	{
		if (string.IsNullOrWhiteSpace(stackName))
		{
			Console.WriteLine("Stack name cannot be empty.");		
		}

		string selectQuery = "SELECT stack_id FROM stacks WHERE name = @stackName";
		string deleteQuery = "DELETE FROM stacks WHERE stack_id = @stack_id";

		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();
				SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
				selectCommand.Parameters.AddWithValue("@stackName", stackName);
				object? stack_id = selectCommand.ExecuteScalar();

				if (stack_id == null)
				{
					Console.WriteLine("Stack not found.");					
				}

				SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
				deleteCommand.Parameters.AddWithValue("@stack_id", stack_id);

				int result = deleteCommand.ExecuteNonQuery();
				if (result > 0)
				{
					Console.WriteLine($"The stack '{stackName}' was successfully deleted.");					
				}
				else
				{
					Console.WriteLine("Failed to delete the stack.");					
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");				
			}
		}
	}

	public void UpdateStack(string stackName, string newStackName)
	{

		if (string.IsNullOrWhiteSpace(stackName) || string.IsNullOrWhiteSpace(newStackName))
		{
			Console.WriteLine("Stack name cannot be empty.");		
		}

		if (stackName == newStackName)
		{
			Console.WriteLine("New stack name cannot be the same as the old stack name.");		
		}

		string query = "UPDATE stacks SET name = @newStackName WHERE name = @stackName";

		using (var connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();

				// Using Dapper to execute the SQL Server query
				int rowsAffected = connection.Execute(query, new { stackName, newStackName });

				// Check if the update was successful
				if (rowsAffected > 0)
				{
					Console.WriteLine("Stack updated successfully.");					
				}
				else
				{
					Console.WriteLine("No stack was updated. Please check if the stack name exists.");
				
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");				
			}
		}
	}
	OperationManager operationManager = new OperationManager();
	public void InsertStacks()
	{
		var stacks = new List<StacksModel>
	{
		new StacksModel("Norwegian"),
		new StacksModel("English"),
		new StacksModel("Spanish"),
		new StacksModel("German")
	};

		if (!operationManager.IsOperationComplete("Stack"))
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					connection.Open();

					foreach (var stack in stacks)
					{
						// Check if the stack already exists in the database
						string checkStackQuery = "SELECT COUNT(*) FROM Stacks WHERE name = @name";
						using (var checkCommand = new SqlCommand(checkStackQuery, connection))
						{
							checkCommand.Parameters.AddWithValue("@name", stack.Name);
							int count = (int)checkCommand.ExecuteScalar();

							// If the stack doesn't exist, insert it
							if (count == 0)
							{
								string insertQuery = "INSERT INTO Stacks (name) VALUES (@name)";
								using (var insertCommand = new SqlCommand(insertQuery, connection))
								{
									insertCommand.Parameters.AddWithValue("@name", stack.Name);
									insertCommand.ExecuteNonQuery();
									Console.WriteLine($"Stack '{stack.Name}' inserted.");
								}
							}
							else
							{
								Console.WriteLine($"Stack '{stack.Name}' already exists. Skipping insert.");
							}
						}
					}

					operationManager.MarkOperationComplete("Stack");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred while inserting stacks. Details: {ex.Message}");
				}
			}
		}
	}


}














