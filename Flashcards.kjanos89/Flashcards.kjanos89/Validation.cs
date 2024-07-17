namespace Flashcards.kjanos89
{
    public class Validation
    {
        public bool ValidateInputForMenu(string input)
        {
            return !String.IsNullOrEmpty(input);
        }
        public bool ValidateInputForMenu(char input)
        {
            return !char.IsLetter(input);
        }
    }
}
