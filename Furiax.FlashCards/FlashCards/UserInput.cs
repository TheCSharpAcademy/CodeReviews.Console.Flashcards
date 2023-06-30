using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards
{
	internal class UserInput
	{
		internal static void ShowMenu()
		{
            Console.WriteLine("Flashcards");
            Console.WriteLine("----------");
			Console.WriteLine("1. Manage Stacks");
            Console.WriteLine("2. Manage Flashcards");
            Console.WriteLine("3. Study");
            Console.WriteLine("4. View Study Data");
            Console.WriteLine("0. Exit");
        }
	}
}
