namespace FlashCards.Model
{
	internal class Helpers
	{
		internal static void MainMenu()
		{
			Console.WriteLine("Flashcards");
			Console.WriteLine("----------");
			Console.WriteLine("1. Manage Stacks");
			Console.WriteLine("2. Manage Flashcards");
			Console.WriteLine("3. Study");
			Console.WriteLine("4. View Study Data");
			Console.WriteLine("0. Exit");
			Console.WriteLine("--------------------");
			Console.WriteLine("Please make a choice by entering the corresponding number: ");
		}
		internal static void StackMenu(string connectionString) 
		{
			DataAccess.ShowStackNames(connectionString);
            Console.WriteLine("---------------------------");
            Console.WriteLine("Input a current stack name");
            Console.WriteLine("Or input 0 to exit input");
            Console.WriteLine("---------------------------");
		}
		internal static void DetailedStackMenu(string stackNameMenu)
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
	}
}
