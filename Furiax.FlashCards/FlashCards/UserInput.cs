using FlashCards.Model;
namespace FlashCards
{
	internal class UserInput
	{
		internal static void DeleteStack(string connectionString, List<Stack> stack)
		{
			Console.WriteLine("Enter the StackId of the stack that you want to delete: ");
			string idToDelete = Console.ReadLine();

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
					DataAccess.UpdateStack(connectionString); Console.ReadLine(); Console.Clear(); 
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
					Console.WriteLine("Value can't be empty");
				else if (DataAccess.DoesStackExist(connectionString, input) == true)
				{
					Console.WriteLine("Can't create stack, a stack with this name already exists");
				}
				else
					validString = true;
			} while (!validString);
			return input;
		}
	}
}
