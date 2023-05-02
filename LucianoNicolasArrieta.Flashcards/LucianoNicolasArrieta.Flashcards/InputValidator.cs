using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucianoNicolasArrieta.Flashcards
{
    public class InputValidator
    {
        private Menu menu = new Menu();
        public string StringInput()
        {
            // For subjects, questions and answers
            string input;

            input = Console.ReadLine();

            while (String.IsNullOrEmpty(input) || input != "0")
            {
                Console.WriteLine("The subject can't be empty. Try again.");
                input = Console.ReadLine();
            }
            if (input == "0")
            {
                Console.Clear();
                menu.RunMain();
            }

            return input;
        }

        public int IdInput()
        {
            string str_input;
            int input;

            str_input = Console.ReadLine();

            while (!Int32.TryParse(str_input, out input))
            {
                Console.WriteLine("Please enter a valid number. Try again.");
                str_input = Console.ReadLine();
            }
            if (input == 0)
            {
                Console.Clear();
                menu.RunMain();
            }

            return input;
        }
    }
}
