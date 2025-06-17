using Flashcards.DTOs;

namespace Flashcards
{
    internal class Validator
    {
        internal static bool StackValidator(string name, List<StackDTO> stacks)
        {
            if (stacks.Exists(stack => stack.Name == name))
            {
                return true;
            }
            return false;
        }



        internal static bool AnswerValidator(string userAnswer, string back)
        {
            if (userAnswer == back)
            {
                return true;
            }
            else return false;
        }

        internal static bool FlashcardValidator(int id, List<FlashcardDTO> flashcards)
        {
            if (id >= 1 && id <= flashcards.Count())
            {
                return true;
            }
            else return false;
        }
    }
}
