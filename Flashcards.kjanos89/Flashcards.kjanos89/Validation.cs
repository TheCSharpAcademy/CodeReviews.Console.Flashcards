namespace Flashcards.kjanos89
{
    public class Validation
    {
        public bool ValidateInputForMenu(string input)
        {
            return !String.IsNullOrEmpty(input);
        }
    }
}
