namespace FlashCards.Model
{
	internal class Helpers
	{
		internal static void MainMenu()
		{
			Console.Clear();
			Console.WriteLine("Flashcards");
			Console.WriteLine("----------");
			Console.WriteLine("Please make a choice by entering the corresponding number: ");
			Console.WriteLine("1. Manage Stacks");
			Console.WriteLine("2. Manage Flashcards");
			Console.WriteLine("3. Study");
			Console.WriteLine("4. View Study Data");
			Console.WriteLine("0. Exit");
			Console.WriteLine("--------------------");
			
		}
		internal static void StackMenu(string connectionString) 
		{
			Console.Clear();
			DataAccess.ShowStackNames(connectionString);
            Console.WriteLine("---------------------------");
			Console.WriteLine("Please make a choice by entering the corresponding number: ");
			Console.WriteLine("1. Create a new stack");
            Console.WriteLine("2. Delete a stack");
            Console.WriteLine("3. Rename a stack");
			Console.WriteLine("0. Return to main menu");
			Console.WriteLine("---------------------------");
		}
		internal static void FlashCardMenu(string stackNameMenu)
		{
			Console.Clear();
			Console.WriteLine("---------------------------");
			Console.WriteLine($"Current working stack: {stackNameMenu}");
            Console.WriteLine();
			Console.WriteLine("0 to return to main menu");
            Console.WriteLine("1 to change current stack");
            Console.WriteLine("2 to view all Flashcards in this stack");
            Console.WriteLine("3 to view X amount of cards in stack");
            Console.WriteLine("4 to Create a Flashcard in current stack");
			Console.WriteLine("5 to Edit a Flashcard");
			Console.WriteLine("6 to Delete a Flashcard");
			Console.WriteLine("---------------------------");
		}
		internal static bool ValidateId(string input)
		{
			if (string.IsNullOrEmpty(input) || !Int32.TryParse(input, out _)) return false;
			if (Int32.Parse(input) < 0) return false;
			return true;
		}
		internal static bool CheckIfRecordExists(string id, List<Stack> stack)
		{
			bool doesExist = false;
			foreach (Stack stackItem in stack)
			{
				if (stackItem.StackId == Convert.ToInt32(id))
					doesExist = true;
			}
			return doesExist;
		}
	}
}
