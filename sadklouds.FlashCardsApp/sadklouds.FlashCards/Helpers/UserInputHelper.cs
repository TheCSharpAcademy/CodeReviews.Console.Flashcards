using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadklouds.FlashCards.Helpers
{
    internal static class UserInputHelper
    {
        public static string GetUserStringInput(string message) 
        {
            Console.Write(message);
            string output = Console.ReadLine();
            return output;
        }

        public static int UserIntInput(string message)
        {
            bool validInput = false;
            int output = 0;
            do
            {
                Console.Write(message);
                string input = Console.ReadLine();
                validInput = int.TryParse(input, out output);
            } while (validInput == false);
            return output;
        }
    }
}
