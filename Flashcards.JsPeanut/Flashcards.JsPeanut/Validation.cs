namespace Flashcards.JsPeanut
{
    class Validation
    {
        public static string ValidateStackOrFlashcard(string stackName)
        {
            string validStack = "valid";
            if (string.IsNullOrWhiteSpace(stackName) || Int32.TryParse(stackName, out _))
            {
                validStack = "invalid";
            }
            return validStack;
        }

        public static string CheckIfThereAreStacks()
        {
            string stacksCount = "greaterThanOne";
            if(CodingController.Stacks.Count() == 0)
            {
                stacksCount = "zero";
            }
            return stacksCount;
        }

        public static string ValidateNumber(string number)
        {
            string validNumber = "valid";
            if (!Int32.TryParse(number, out _) || Convert.ToInt32(number) < 0)
            {
                validNumber = "invalid";
            }
            return validNumber;
        }
    }
}
