using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdityaFlashCards.Helper

{
    internal class InputValidator
    {
        internal static bool IsGivenInputInteger(string input)
        {
            if (int.TryParse(input, out _))
                return true;
            else
                return false;
        }

    }
}
