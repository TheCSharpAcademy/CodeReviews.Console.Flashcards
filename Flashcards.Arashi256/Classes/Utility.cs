
namespace Flashcards.Arashi256.Classes
{
    internal class Utility
    {
        public static string TranslateSortOrderToString(SortOrder sortOrder)
        {
            return sortOrder == SortOrder.ASC ? "ASC" : "DESC";
        }

        public enum SortOrder
        {
            ASC, DESC
        }
    }
}