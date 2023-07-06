using ConsoleTableExt;
using FlashCards.Model;
using System.Data.SqlClient;

namespace FlashCards
{
	internal class DataAccess
	{
		internal static void Flashcards()
		{
			throw new NotImplementedException();
		}

		internal static void ShowStackNames(string connectionString)
		{
			List<StackNameDTO> stackNames = BuildStackDTO(connectionString);
			ConsoleTableBuilder
					.From(stackNames)
					.WithColumn("Overview Stack Names")
					.ExportAndWriteLine();
            }

		internal static List<StackNameDTO> BuildStackDTO(string connectionString)
		{
			List<StackNameDTO> stackNames = new();
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = "SELECT StackName FROM dbo.Stack";

				SqlDataReader reader = sqlCommand.ExecuteReader();
				if (reader.HasRows)
				{
					stackNames.Clear();
					while (reader.Read())
					{
						stackNames.Add(new StackNameDTO
						{
							StackName = reader.GetString(0)
						});
					}
				}
				else
					Console.WriteLine("No records found");
				connection.Close();
			}
			return stackNames;
	}

		internal static void Stack(string connectionString)
		{
			Console.Clear();
			UserInput.GetStackMenuInput(connectionString);
		}

		internal static void CreateNewStack(string connectionString)
		{
			Console.Clear();
			string stackName = UserInput.NewStack(connectionString);
			using(var connection = new SqlConnection(connectionString))
			{ 
				connection.Open(); 
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = "INSERT INTO dbo.Stack (StackName) VALUES (@stackName)";
				sqlCommand.Parameters.Add(new SqlParameter("@stackName", stackName));
				sqlCommand.ExecuteNonQuery();
				connection.Close();
			}
            Console.WriteLine($"New stack {stackName} created");
        }

		internal static void DeleteStack(string connectionString)
		{
            Console.WriteLine("Stack deleted");
        }

		internal static void UpdateStack(string connectionString)
		{
            Console.WriteLine("Stack renamed");
        }
	}
}
