using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jollejonas.Flashcards.Validation
{
    public class InputValidation
    {
        public static bool ValidateString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return true;
        }

    }
}
