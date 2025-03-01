namespace vcesario.Flashcards;

public static class ApplicationTexts
{
    public const string ACTION_PROMPT = "What do you want to do?";

    public const string CREATENEWSTACK_HEADER = "Create new stack";
    public const string CREATENEWSTACK_PROMPT = "New stack name:";
    public const string CREATENEWSTACK_LOG = "New stack {0} created.";

    public const string USERINPUT_DATETIMEERROR = "Couldn't parse DateTime. Use provided template.";
    public const string USERINPUT_DATEERROR = "Couldn't parse Date. Use provided template.";
    public const string USERINPUT_OLDERDATEERROR = "Cannot accept dates older than today.";
    public const string USERINPUT_LONGERROR = "Couldn't parse number. Try again.";
}