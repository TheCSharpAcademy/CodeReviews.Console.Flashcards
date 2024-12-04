

namespace Flashcards.Bina28;

internal class Helper
{
	internal  int GetValidIntegerInput(string prompt)
	{
		int result;
		string input;

		do
		{
			Console.WriteLine(prompt);
			input = Console.ReadLine();
		} while (!int.TryParse(input, out result));
		return result;
	}

	internal string GetNonEmptyInput(string prompt)
	{
		string input;
		do
		{
			Console.WriteLine(prompt);
			input = Console.ReadLine()?.Trim(); 

			if (string.IsNullOrEmpty(input))
			{
				Console.WriteLine("Input cannot be empty. Please try again.");
			}
		}
		while (string.IsNullOrEmpty(input));

		return input;
	}

	internal void WaitForKeyPress()
	{
		Console.WriteLine("\nPress any key to return to the main menu...");
		Console.ReadKey();
	}
}
