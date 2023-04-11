namespace FlashCards;

public static class Helpers
{
    public static string DisplayError(string error)
    {
        string arrowLeft = "\n|---> ";
        string arrowRight = " <---|\n";
        return $"{arrowLeft} {error} {arrowRight}";
    }

    public static string SafeTextSql(string text)
    {
        text = text.Replace("'", " ");
        return text;
    }
}
