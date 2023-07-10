using ConsoleTableExt;
using FlashCards.Model;
using System.Data.SqlClient;

namespace FlashCards
{
	internal class DataAccess
	{
		internal static void Stack(string connectionString)
		{
			UserInput.GetStackMenuInput(connectionString);
		}
		internal static void Flashcards(string connectionString)
		{
			Console.Clear();
			UserInput.GetStackName(connectionString);
		}
		internal static void ShowStackNames(string connectionString)
		{
			List<StackNameDTO> stackNames = BuildStackDTO(connectionString);
			ConsoleTableBuilder
					.From(stackNames)
					.WithColumn("List of Stack Names")
					.ExportAndWriteLine();
		}
		internal static List<Stack> BuildStack(string connectionString, string command)
		{
			List<Stack> stack = new();
			using var connection = new SqlConnection(connectionString);
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = command;
				SqlDataReader reader = sqlCommand.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						stack.Add(new Stack
						{
							StackId = reader.GetInt32(0),
							StackName = reader.GetString(1)
						}) ;
					}
				}
			}
			return stack;
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
		internal static bool DoesStackExist(string connectionString, string inputName)
		{
			bool doesExist = false;
			List<StackNameDTO> existingStacks = new List<StackNameDTO>();
			existingStacks = BuildStackDTO(connectionString);
			foreach (var stack in existingStacks)
			{
				if (inputName.ToLower() ==  stack.StackName.ToLower())
				{
					doesExist = true;
				}
			}
			return doesExist;
		}
		internal static void DeleteStack(string connectionString)
		{
			Console.Clear();
			string command = "SELECT * from dbo.Stack";
			List<Stack> stack = BuildStack(connectionString,command);
			ConsoleTableBuilder
				.From(stack)
				.WithTitle("Stacks")
				.ExportAndWriteLine();
			string stackId = UserInput.DeleteStack(connectionString, stack);
			if (stackId == "0")
			{
				Stack(connectionString);
			}
			else
			{
				using (var connection = new SqlConnection(connectionString))
				{
					connection.Open();
					var sqlCommand = connection.CreateCommand();
					sqlCommand.CommandText = "DELETE FROM dbo.Stack WHERE StackId = (@stackId)";
					sqlCommand.Parameters.Add(new SqlParameter("@stackId", stackId));
					sqlCommand.ExecuteNonQuery();
					connection.Close();
				}
				Console.WriteLine("Stack succesfully deleted");
				// in SSMS check 'Cascade' box at the 'INSERT and UPDATE SPECIFICATIONS' option when adding FK reference so all references in all tables are deleted
			}
        }
		internal static void RenameStack(string connectionString)
		{
			Console.Clear();
			string command = "SELECT * from dbo.Stack";
			List<Stack> stack = BuildStack(connectionString, command);
			ConsoleTableBuilder
				.From(stack)
				.WithTitle("Stacks")
				.ExportAndWriteLine();
			var newStackInfo = UserInput.RenameStack(connectionString, stack);
			if (newStackInfo.idToRename == "0")
			{
				Stack(connectionString);
			}
			else
			{
				using (var connection = new SqlConnection(connectionString))
				{
					connection.Open();
					var sqlCommand = connection.CreateCommand();
					sqlCommand.CommandText = "UPDATE dbo.Stack SET StackName = (@stackName) WHERE StackId = (@stackId)";
					sqlCommand.Parameters.Add(new SqlParameter("@stackId", newStackInfo.idToRename));
					sqlCommand.Parameters.Add(new SqlParameter("@stackName", newStackInfo.newName));
					sqlCommand.ExecuteNonQuery();
					connection.Close();
				}
				Console.WriteLine("Stack succesfully renamed");
			}
        }
		internal static List<FlashcardDTO> BuildFlashcardDTO(string connectionString, string stackId)
		{
			List<FlashcardDTO> flashcards = new();
			using(var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = "SELECT FlashcardId, FrontText, BackText from dbo.Flashcard WHERE StackId = @stackId";
				sqlCommand.Parameters.Add(new SqlParameter("@stackId", stackId));
				SqlDataReader reader = sqlCommand.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						flashcards.Add(new FlashcardDTO
						{
							FlashcardId = reader.GetInt32(0),
							FrontText = reader.GetString(1),
							BackText = reader.GetString(2)
						});
					}
				}
			}
			return flashcards;
		}
		internal static void ShowAllFlashCards(string connectionString, string stackName, string stackId)
		{
			Console.Clear();
			List<FlashcardDTO> flashcards = BuildFlashcardDTO(connectionString, stackId);
			if (flashcards.Count == 0)
				Console.WriteLine("No flashcards found for this stack");
			else
			{
				ConsoleTableBuilder
					.From(flashcards)
					.WithTitle(stackName)
					.WithColumn("Id", "Front", "Back")
					.ExportAndWriteLine();
			}
        }
	}
}
