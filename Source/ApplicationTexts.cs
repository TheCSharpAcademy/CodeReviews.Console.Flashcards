namespace vcesario.Flashcards;

public static class ApplicationTexts
{
    public const string ACTION_PROMPT = "What do you want to do?";
    public const string OPTION_RETURN = "Return";

    public const string CREATENEWSTACK_HEADER = "Create new stack";
    public const string CREATENEWSTACK_PROMPT = "New stack name:";
    public const string CREATENEWSTACK_LOG = "New stack {0} created.";
    public const string STACKSMANAGER_HEADER = "Manage stacks";
    public const string STACKSMANAGER_PROMPT = "Select a stack to manage";
    public const string STACKSMANAGER_HEADER_SINGLE = "Manage stack: {0}";
    public const string STACKSMANAGER_LOG_DEBUGCREATED = "Debug cards created.";

    public const string USERINPUT_DATETIMEERROR = "Couldn't parse DateTime. Use provided template.";
    public const string USERINPUT_DATEERROR = "Couldn't parse Date. Use provided template.";
    public const string USERINPUT_OLDERDATEERROR = "Cannot accept dates older than today.";
    public const string USERINPUT_LONGERROR = "Couldn't parse number. Try again.";
}