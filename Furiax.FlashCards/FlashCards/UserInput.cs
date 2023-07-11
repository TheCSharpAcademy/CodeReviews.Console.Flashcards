using ConsoleTableExt;
using FlashCards.Model;

namespace FlashCards
{
	internal class UserInput
	{
		internal static void GetMenuInput(string connectionString)
		{
			Console.Clear();
			bool closeApp = false;
			while (!closeApp)
			{
				Helpers.MainMenu();
				string input = Console.ReadLine();
				switch (input)
				{
					case "1":
						DataAccess.Stack(connectionString);
						break;
					case "2":
						DataAccess.Flashcards(connectionString);
						break;
					case "3":
						DataAccess.Study(connectionString);
						break;
					case "4":
						//ViewStudyData()
						; Console.ReadLine(); Console.Clear();
						break;
					case "0":
						Console.WriteLine("Goodbye");
						Environment.Exit(0);
						closeApp = true;
						break;
					default:
						Console.WriteLine("Invalid input");
						Console.ReadLine();
						Console.Clear();
						break;
				}
			}
		}
		internal static void GetStackMenuInput(string connectionString)
		{
			Console.Clear();
			Helpers.StackMenu(connectionString);
			string input = Console.ReadLine();
			switch (input)
			{
				case "1":
					DataAccess.CreateNewStack(connectionString); Console.ReadLine(); Console.Clear();
					break;
				case "2":
					DataAccess.DeleteStack(connectionString); Console.ReadLine(); Console.Clear();
					break;
				case "3":
					DataAccess.RenameStack(connectionString); Console.ReadLine(); Console.Clear();
					break;
				case "0":
					GetMenuInput(connectionString);
					break;
				default:
					Console.WriteLine("Invalid input");
					Console.ReadKey();
					GetStackMenuInput(connectionString);
					break;
			}
		}
		internal static void GetFlashCardMenuInput(string connectionString, string stackName, string stackId)
		{
			Console.Clear();
			Helpers.FlashCardMenu(stackName);
			string inputFlashCardMenu = Console.ReadLine();
			switch (inputFlashCardMenu)
			{
				case "0":
					GetMenuInput(connectionString); break;
				case "1":
					GetStackName(connectionString); break;
				case "2":
					DataAccess.ShowAllFlashcards(connectionString, stackName, stackId); Console.ReadKey(); break;
				case "3":
					DataAccess.ShowXFlashcards(connectionString, stackName, stackId); Console.ReadKey(); break;
				case "4":
					DataAccess.CreateFlashcard(connectionString, stackId); break;
				case "5":
					DataAccess.ModifyFlashcard(connectionString, stackId); break;
				case "6":
					DataAccess.DeleteFlashcard(connectionString, stackId); break;
				default:
					Console.WriteLine("Invalid input");
					Console.ReadKey();
					GetFlashCardMenuInput(connectionString, stackName, stackId);
					break;
			}
		}
		internal static (string stackName, string stackId) GetStackName(string connectionString)
		{
			Console.Clear();
			string command = "SELECT * from dbo.Stack";
			List<Stack> stack = DataAccess.BuildStack(connectionString, command);
			ConsoleTableBuilder
				.From(stack)
				.WithTitle("Stacks")
				.ExportAndWriteLine();
			string stackName = "";
			string stackId = "";
			while (true)
			{
				Console.WriteLine("Enter the id number of the stack that you want to work with or 0 to return:");
				stackId = Console.ReadLine();

				if (stackId == "0")
				{
					Console.Clear(); GetMenuInput(connectionString); break;
				}
				else if (Helpers.ValidateId(stackId) && Helpers.CheckIfRecordExists(stackId, stack))
				{
					stackName = "";
					foreach (Stack stackItem in stack)
					{
						if (stackItem.StackId == Convert.ToInt32(stackId))
							stackName = stackItem.StackName;
					}
					break;
				}
				else
				{
					Console.WriteLine("This stack does not exist, please enter the correct name");
				}
			}
			return (stackName, stackId);
		}
		internal static string NewStack(string connectionString)
		{
			bool validString = false;
			string input = "";
			do
			{
				Console.WriteLine("Please enter a name for the new stack or enter 0 to return: ");
				input = Console.ReadLine();
				if (input == "0")
				{
					GetStackMenuInput(connectionString);
					break;
				}
				else if (input.Trim() == "")
					Console.WriteLine("Stack name can't be empty");
				else if (DataAccess.DoesStackExist(connectionString, input) == true)
				{
					Console.WriteLine("A stack with that name already exists, please choose another one");
				}
				else
					validString = true;

			} while (!validString);
			return input;
		}
		internal static string DeleteStack(string connectionString, List<Stack> stack)
		{
			while (true)
			{
				Console.WriteLine("Enter the stack id of the stack that you want to delete or 0 to return:");
				string idToDelete = Console.ReadLine();
				if (idToDelete == "0" || (Helpers.ValidateId(idToDelete) == true && Helpers.CheckIfRecordExists(idToDelete, stack) == true))
				{
					return idToDelete;
				}
				else
					Console.WriteLine("Not a valid StackId, try again");
			}
		}
		internal static (string idToRename, string newName) RenameStack(string connectionString, List<Stack> stack)
		{
			bool validId = false;
			bool validName = false;
			string idToRename = "";
			string newName = "";
			while (validId == false)
			{
				Console.WriteLine("Enter the stack id of the stack that you want to rename or 0 to return:");
				idToRename = Console.ReadLine();
				if (idToRename == "0" || (Helpers.ValidateId(idToRename) == true && Helpers.CheckIfRecordExists(idToRename, stack) == true))
				{
					validId = true;
				}
				else
					Console.WriteLine("Not a valid StackId, try again");
			}

			while (validName == false)
			{
				Console.WriteLine("Please enter a new name for the stack:");
				newName = Console.ReadLine();
				if (newName.Trim() == "")
					Console.WriteLine("Stack name can't be emty");
				else if (DataAccess.DoesStackExist(connectionString, newName) == true)
					Console.WriteLine("A stack with that name already exists, please choose another one");
				else
					validName = true;
			}
			return (idToRename, newName);
		}
		internal static string GetFlashCardFront()
		{
			bool validFrontText = false;
			string frontText = "";
			while (validFrontText == false)
			{
				Console.WriteLine("Enter the text for the frontside: ");
				frontText = Console.ReadLine();
				if (!string.IsNullOrEmpty(frontText.Trim()))
					validFrontText = true;
				else
					Console.WriteLine("Value can't be empty, please enter a stringvalue");
			}
			return frontText;
		}
		internal static string GetFlashCardBack()
		{
			bool validBackText = false;
			string backText = "";
			while (validBackText == false)
			{
				Console.WriteLine("Enter the text for the backside: ");
				backText = Console.ReadLine();
				if (!string.IsNullOrEmpty(backText.Trim()))
					validBackText = true;
				else
					Console.WriteLine("Value can't be empty, please enter a stringvalue");
			}
			return backText;
		}
		internal static void GetStudyMenuInput(string connectionString, string stackName, string stackId)
		{
			Console.Clear();
			Helpers.StudyMenu(stackName);
			string input = Console.ReadLine();
			switch(input)
			{
				case "0":
					break;
				case "1":
					DataAccess.ShowAllFlashcards(connectionString, stackName, stackId); 
					Console.ReadKey();
					break;
				case "2":
					DataAccess.TakeQuiz(connectionString, stackName, stackId);
					Console.ReadKey(); 
					break;
				default:
					Console.WriteLine("Invalid input");
					Console.ReadKey();
					GetStudyMenuInput(connectionString, stackName, stackId); break;
			}
		}
		internal static string GetStudyAnswer()
		{
			Console.WriteLine("Input your answer to this card or 0 to exit: ");
			string answer = Console.ReadLine();
			return answer;
		}
	}
}
