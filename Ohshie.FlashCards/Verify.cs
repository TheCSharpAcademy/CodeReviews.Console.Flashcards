namespace Ohshie.FlashCards;

public static class Verify
{
    public static bool StackExist()
    {
        DbOperations dbOperations = new();
        var stacks = dbOperations.FetchAllDecks();
        if (!stacks.Any()) return false;

        return true;
    }
}