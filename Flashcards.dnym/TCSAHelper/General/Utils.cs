namespace TCSAHelper.General;

public static class Utils
{
    public static string LimitWidth(string s, int width, string ellipsis = "...")
    {
        if (s.Length <= width || width < ellipsis.Length)
        {
            return s;
        }
        else
        {
            return string.Concat(s.AsSpan(0, width - ellipsis.Length), ellipsis);
        }
    }
}
