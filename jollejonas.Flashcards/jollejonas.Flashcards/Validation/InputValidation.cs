﻿namespace jollejonas.Flashcards.Validation
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