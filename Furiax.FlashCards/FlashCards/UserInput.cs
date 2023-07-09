using FlashCards.Model;
namespace FlashCards
{
	internal class UserInput
	{
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
						DataAccess.Flashcards(); Console.ReadLine(); Console.Clear();
						break;
					case "3":
						/*Study()*/
						; Console.ReadLine(); Console.Clear();
						break;
					case "4":
						/*ViewStudyData()*/
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
					DataAccess.CreateNewStack(connectionString); Console.ReadLine();  Console.Clear();
					break;
				case "2":
					DataAccess.DeleteStack(connectionString); Console.ReadLine() ; Console.Clear();
					break;
				case "3":
					DataAccess.RenameStack(connectionString); Console.ReadLine(); Console.Clear(); 
					break;
				case "0":
					GetMenuInput(connectionString); 
					break;
			}
		}
		internal static void GetStackName(string connectionString)
		{
			string input = Console.ReadLine();
			bool isInputInStackNames = false;
			string stackNameMenu = "";
			List<StackNameDTO> stackNames = DataAccess.BuildStackDTO(connectionString);

			foreach (StackNameDTO stackName in stackNames)
			{
				if (input.ToLower() == stackName.StackName.ToLower())
				{
					isInputInStackNames = true;
					stackNameMenu = stackName.StackName;
				}
			}
			if (isInputInStackNames)
			{
				Helpers.FlashCardMenu(stackNameMenu);
			}
			else
			{
				if (input == "0")
				{
					Console.Clear(); GetMenuInput(connectionString);
				}
				else
				{
					Console.WriteLine("Invalid choice");
				}
			} 
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
				if(idToRename == "0" || (Helpers.ValidateId(idToRename) == true && Helpers.CheckIfRecordExists(idToRename, stack) == true))
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
	}
}
