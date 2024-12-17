

using Microsoft.Data.SqlClient;

namespace Flashcards.Bina28.DBmanager;


	internal class OperationManager
	{
		private string connectionString;

		public OperationManager()
		{
			// Hent tilkoblingsstrengen fra App.config
			connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
		}

		// Sjekk statusen før insert
		public bool IsOperationComplete(string name)
		{
			string query = "SELECT status FROM OperationStatus WHERE name = @name";

			using (var connection = new SqlConnection(connectionString))
			{
				try
				{
					connection.Open();
					using (var command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@name", name);
						var result = command.ExecuteScalar();

						if (result != null)
						{
							return Convert.ToInt32(result) == 1;  // Returner true hvis operasjonen er vellykket
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"En feil oppstod ved sjekk av operasjonsstatus: {ex.Message}");
				}
			}

			return false;  // Hvis operasjonen ikke finnes eller en feil oppstod
		}

	// Sett status til 1 etter vellykket insert
	public void MarkOperationComplete(string name)
	{
		string query = @"
    IF NOT EXISTS (SELECT 1 FROM OperationStatus WHERE name = @name)
    BEGIN
        INSERT INTO OperationStatus (name, status) VALUES (@name, 0); -- Initial status set to 0
    END
    UPDATE OperationStatus SET status = 1 WHERE name = @name";

		using (var connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();
				using (var command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@name", name);
					command.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while updating the operation status: {ex.Message}");
			}
		}
	}

	// Metode for å opprette tabellen 'OperationStatus' hvis den ikke finnes
	public void CreateOperationStatusTable()
		{
		string createTableQuery = @"
    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'OperationStatus' AND xtype = 'U')
        CREATE TABLE OperationStatus (
            name NVARCHAR(255) NOT NULL,
            status INT NOT NULL,  
            PRIMARY KEY (name)
        );";



		using (var connection = new SqlConnection(connectionString))
			{
				try
				{
					connection.Open();
					using (var command = new SqlCommand(createTableQuery, connection))
					{
						command.ExecuteNonQuery();
					
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred: {ex.Message}");
				}
			}
		}
	}
