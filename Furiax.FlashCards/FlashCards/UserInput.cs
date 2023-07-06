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
						DataAccess.Stack(connectionString); Console.ReadLine(); Console.Clear();
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
			Console.WriteLine("Please enter a name for the new stack: ");
			string input = Console.ReadLine();
			// todo: add validation on valid name and on already exists
			return input;
		}
	}
}
