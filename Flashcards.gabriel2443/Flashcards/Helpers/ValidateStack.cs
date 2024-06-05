using Flashcards.Database;

namespace Flashcards.Helpers
{
    public class ValidateStack
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
    }
}