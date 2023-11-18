namespace Flashcards.Core;

internal static class StackManagement
{
    public static string CreateSortName(string viewName)
    {
        return viewName.Trim().ToLower();
    }
}
