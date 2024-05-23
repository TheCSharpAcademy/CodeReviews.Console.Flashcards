using Flashcards.Database;

namespace Flashcards.Helpers
{
    public class ValidateFlashcardsStacks
    {
        internal static bool StackExists(string stackName)
        {
            var stackDatabaseManager = new StackDatabaseManager();
            var stacks = stackDatabaseManager.GetStacks();

            var isSameStack = false;
            foreach (var stack in stacks)
            {
                if (stackName.ToLower() == stack.CardstackName.ToLower()) isSameStack = true;
            }

            return isSameStack;
        }

        internal static bool InputFront(string flashCardQuestion)
        {
            var stack = new StackDatabaseManager();
            var getFlashCards = new FlashcardDatabaseManager();
            var stackId = stack.GetStackById();
            var flashcards = getFlashCards.ReadFlahcards(stackId);
            var isSameStack = false;

            foreach (var flashcard in flashcards)
            {
                if (flashCardQuestion.ToLower().Trim() == flashcard.Question.ToLower().Trim()) isSameStack = true;
            }
            return isSameStack;
        }

        internal static bool InputBack(string flashCardAnswer)
        {
            var stack = new StackDatabaseManager();
            var getFlashCards = new FlashcardDatabaseManager();
            var stackId = stack.GetStackById();
            var flashcards = getFlashCards.ReadFlahcards(stackId);
            var isSameStack = false;

            foreach (var flashcard in flashcards)
            {
                if (flashCardAnswer.ToLower().Trim() == flashcard.Answer.ToLower().Trim()) isSameStack = true;
            }
            return isSameStack;
        }
    }
}