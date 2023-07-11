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
			var stackInfo = UserInput.GetStackName(connectionString);
			UserInput.GetFlashCardMenuInput(connectionString, stackInfo.stackName, stackInfo.stackId);
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
		internal static List<FlashcardDTO> BuildFlashcardDTOcustomId(string connectionString, string stackId)
		{
			List<FlashcardDTO> flashcards = new();
			using(var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = "SELECT FrontText, BackText from dbo.Flashcard WHERE StackId = @stackId";
				sqlCommand.Parameters.Add(new SqlParameter("@stackId", stackId));
				SqlDataReader reader = sqlCommand.ExecuteReader();
				if (reader.HasRows)
				{
					int id = 1;
					while (reader.Read())
					{
						flashcards.Add(new FlashcardDTO
						{
							Id = id,
							FrontText = reader.GetString(0),
							BackText = reader.GetString(1)
						}) ;
						id++;
					}
				}
			}
			return flashcards;
		}
		internal static List<FlashcardDTO> BuildFlashcardDTO(string connectionString, string stackId)
		{
			List<FlashcardDTO> flashcards = new();
			using (var connection = new SqlConnection(connectionString))
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
							Id = reader.GetInt32(0),
							FrontText = reader.GetString(1),
							BackText = reader.GetString(2)
						});
					}
				}
			}
			return flashcards;
		}
		internal static void ShowAllFlashcards(string connectionString, string stackName, string stackId)
		{
			Console.Clear();
			List<FlashcardDTO> flashcards = BuildFlashcardDTOcustomId(connectionString, stackId);
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
		internal static void ShowXFlashcards(string connectionString, string stackName, string stackId)
		{
			Console.Clear();
			List<FlashcardDTO> flashcards = BuildFlashcardDTOcustomId(connectionString, stackId);
			if (flashcards.Count == 0)
				Console.WriteLine($"The stack {stackName} doesn't contain any flashcards");
			else
			{
				Console.WriteLine($"The stack {stackName} contains {flashcards.Count} flashcards");
				bool validInt = false;
				while (validInt == false)
				{
					Console.WriteLine("How many would you like to display ?");
					string input = Console.ReadLine();
					if (Helpers.IsValidInt(input))
					{
						int number = int.Parse(input);
						List<FlashcardDTO> xFlashcards = new();
						using (var connection = new SqlConnection(connectionString))
						{
							connection.Open();
							var sqlcommand = connection.CreateCommand();
							sqlcommand.CommandText = "SELECT TOP (@number) FrontText, BackText from dbo.Flashcard WHERE StackId = @stackId";
							sqlcommand.Parameters.Add(new SqlParameter("@number", number));
							sqlcommand.Parameters.Add(new SqlParameter("@stackId", stackId));
							SqlDataReader reader = sqlcommand.ExecuteReader();
							if (reader.HasRows)
							{
								int id = 1;
								while (reader.Read())
								{
									xFlashcards.Add(new FlashcardDTO
									{
										Id = id,
										FrontText = reader.GetString(0),
										BackText = reader.GetString(1)
									});
									id++;
								}
							}
						}
						ConsoleTableBuilder
							.From(xFlashcards)
							.WithTitle(stackName)
							.WithColumn("Id", "Front", "Back")
							.ExportAndWriteLine();
						validInt = true;
					}
					else
					{
						Console.WriteLine("Not a valid number try again");
					}
				}
			}
		}
		internal static void CreateFlashcard(string connectionString, string stackId)
		{
			Console.Clear();
			Console.WriteLine("Create new flashcard:");
			Console.WriteLine("---------------------");
			string frontText = UserInput.GetFlashCardFront();
			string backText = UserInput.GetFlashCardBack();
			using(var connection = new SqlConnection(connectionString))
			{ 
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = "INSERT INTO dbo.FlashCard (FrontText, BackText, StackId) VALUES (@frontText, @backText, @stackId)";
				sqlCommand.Parameters.Add(new SqlParameter("@frontText", frontText));
				sqlCommand.Parameters.Add(new SqlParameter("@backText", backText));
				sqlCommand.Parameters.Add(new SqlParameter("@stackId", stackId));
				sqlCommand.ExecuteNonQuery();
				connection.Close();
			}
		}
		internal static void ModifyFlashcard(string connectionString, string stackId)
		{
			Console.Clear();
			List<FlashcardDTO> flashcards = BuildFlashcardDTO(connectionString, stackId);
			ConsoleTableBuilder
				.From(flashcards)
				.WithTitle("Flashcards")
				.WithColumn("Id","Front", "Back")
				.ExportAndWriteLine();
			bool validId = false;
			while (validId == false)
			{
				Console.WriteLine("Enter the id of the card you want to edit or 0 to return: ");
				string flashcardId = Console.ReadLine();
				if (flashcardId == "0")
					break;
				else if (Helpers.ValidateId(flashcardId) && Helpers.DoesFlashcardIdExists(flashcardId, flashcards))
				{
					validId = true;
					string frontText = UserInput.GetFlashCardFront();
					string backText = UserInput.GetFlashCardBack();
					using (var connection = new SqlConnection(connectionString))
					{
						connection.Open();
						var sqlCommand = connection.CreateCommand();
						sqlCommand.CommandText = "UPDATE dbo.FlashCard SET FrontText = @frontText, BackText = @backText WHERE FlashcardId = (@flashcardId)";
						sqlCommand.Parameters.Add(new SqlParameter("@frontText", frontText));
						sqlCommand.Parameters.Add(new SqlParameter("@backText", backText));
						sqlCommand.Parameters.Add(new SqlParameter("@flashcardId", flashcardId));
						sqlCommand.ExecuteNonQuery();
						connection.Close();
					}
				}
				else
					Console.WriteLine("A flashcard with this id does not exist, try again");
            }
		}
		internal static void DeleteFlashcard(string connectionString, string stackId)
		{
			Console.Clear();
			List<FlashcardDTO> flashcards = BuildFlashcardDTO(connectionString, stackId);
			ConsoleTableBuilder
				.From(flashcards)
				.WithTitle("Flashcards")
				.WithColumn("Id", "Front", "Back")
				.ExportAndWriteLine();
			bool validId = false;
			while (validId == false)
			{
				Console.WriteLine("Enter the id of the card you want to delete or 0 to return: ");
				string flashcardId = Console.ReadLine();
				if (flashcardId == "0")
					break;
				else if (Helpers.ValidateId(flashcardId) && Helpers.DoesFlashcardIdExists(flashcardId, flashcards))
				{
					validId = true;
					using (var connection = new SqlConnection(connectionString))
					{
						connection.Open();
						var sqlCommand = connection.CreateCommand();
						sqlCommand.CommandText = "DELETE FROM dbo.Flashcard WHERE FlashcardId = (@flashcardId)";
						sqlCommand.Parameters.Add(new SqlParameter("@flashcardId", flashcardId));
						sqlCommand.ExecuteNonQuery();
						connection.Close();
					}
				}
				else
					Console.WriteLine("A flashcard with that id does not exist, please try again");
			}
		}
		internal static void Study(string connectionString)
		{
			Console.Clear();
			var stackInfo = UserInput.GetStackName(connectionString);
			UserInput.GetStudyMenuInput(connectionString, stackInfo.stackName, stackInfo.stackId);
		}
		internal static void TakeQuiz(string connectionString, string stackName, string stackId)
		{
			Console.Clear();
			List<FlashcardDTO> flashcardList = BuildFlashcardDTO(connectionString, stackId);
			int score = 0;
			foreach (var item in flashcardList)
			{
				List<StudyFrontDTO> studyList = new();
				studyList.Add(new StudyFrontDTO
				{
					Front = item.FrontText
				});
				ConsoleTableBuilder
					.From(studyList)
					.WithTitle(stackName)
					.WithColumn("Front")
					.ExportAndWriteLine();

				string input = UserInput.GetStudyAnswer();
				string answer = item.BackText;

				if (input == "0")
				{
					UserInput.GetMenuInput(connectionString);
					break;
				}
				else if (input.ToLower() == answer.ToLower())
				{
					Console.WriteLine("Your answer is correct !!");
					score++;
				}
				else
				{
					Console.WriteLine("Your answer was wrong.");
                    Console.WriteLine($"The correct answer was {answer}");
                }
				Console.ReadKey();
				Console.Clear();
			}

			CreateStudySession(connectionString, stackId, score);
            Console.WriteLine("Exiting Study session");
			Console.WriteLine($"You got {score} right out of {flashcardList.Count}");
			Console.ReadKey();
        }
		private static void CreateStudySession(string connectionString, string stackId, int score)
		{
			using(var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = "INSERT INTO dbo.StudySession (StackId, Score) VALUES (@stackId, @score)";
				sqlCommand.Parameters.Add(new SqlParameter("@stackId", stackId));
				sqlCommand.Parameters.Add(new SqlParameter("@score", score));
				sqlCommand.ExecuteNonQuery();
				connection.Close();
			}
		}
		internal static void ViewStudyData(string connectionString)
		{
			Console.Clear();
			List<StudySession> sessions = new();
			using(var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = "SELECT StudySessionId, StackId, StudyDate, Score FROM dbo.StudySession";
				SqlDataReader reader = sqlCommand.ExecuteReader();
				if (reader.HasRows)
				{
					while(reader.Read())
					{
						sessions.Add(new StudySession
						{
							StudySessionId = reader.GetInt32(0),
							StackId = reader.GetInt32(1),
							StudyDate = reader.GetDateTime(2),
							Score = reader.GetInt32(3),
						}) ;
					}
				}
			}
			ConsoleTableBuilder
				.From(sessions)
				.WithTitle("Study Sessions")
				.WithColumn("Id", "StackId", "Date", "Score")
				.WithFormatter(2, f => $"{f:dd/MM/yy}")
				.ExportAndWriteLine();
			Console.ReadKey();
		}
	}
}
