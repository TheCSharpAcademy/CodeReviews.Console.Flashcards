namespace Utilities.GoldRino456;
public static class DebugUtils
{
    public static void PrintListToConsole<T>(List<T> list)
    {
        foreach (var item in list)
        {
            Console.WriteLine(item);
        }
    }
}
