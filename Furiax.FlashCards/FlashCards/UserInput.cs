using FlashCards.Model;
namespace FlashCards
{
	internal class UserInput
	{
        internal static void GetMenuInput(string connectionString)
        {
			bool closeApp = false;
			while (!closeApp)
			{
				Helpers.MainMenu();
				string menuChoice = Console.ReadLine();
				switch (menuChoice)
				{
					case "1":
						DataAccess.Stacks(connectionString); Console.ReadLine(); Console.Clear();
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
						closeApp = true;
						Console.WriteLine("Goodbye");
						Environment.Exit(0);
						break;
					default:
						Console.WriteLine("Invalid input");
						Console.ReadLine();
						Console.Clear();
						break;
				} 
			}
		}
		internal static void GetStackInput(string connectionString)
		{

			while (true)
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
					Helpers.DetailedStackMenu(stackNameMenu);
					break;
				}
				else
				{
					if (input == "0")
					{
						Console.Clear(); GetMenuInput(connectionString);break;
					}
					else
					{
						Console.WriteLine("Invalid choice");
					}
				} 
			}
		}
	}
}
