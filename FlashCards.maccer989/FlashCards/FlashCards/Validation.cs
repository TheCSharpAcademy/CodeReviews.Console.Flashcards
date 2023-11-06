namespace FlashCardsUI
{
    public class Validation
    {
        public static int CheckValidNumber(string userInput)
        {
            int output;
            int number;
            while ((!Int32.TryParse(userInput, out number)) || ((number < 1) || (number > 9)))
            {
                Console.WriteLine("Please enter a valid number use numbers 1 to 9 only:");
                userInput = Console.ReadLine();                
            }
            output = number;
            return output; 
        }
    }
}
