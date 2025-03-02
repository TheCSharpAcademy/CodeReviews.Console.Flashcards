namespace vcesario.Flashcards;

public static class ApplicationTexts
{
    public const string OPTION_RETURN = "Return";
    public const string PROMPT_ACTION = "What do you want to do?";
    public const string PROMPT_REALLYDELETE = "Are you REALLY sure?";

    public const string CREATENEWSTACK_HEADER = "Create new stack";
    public const string CREATENEWSTACK_PROMPT = "New stack name:";
    public const string CREATENEWSTACK_LOG = "New stack {0} created.";

    public const string STACKSMANAGER_HEADER = "Manage stacks";
    public const string STACKSMANAGER_PROMPT_SELECTSTACK = "Select a stack to manage";
    public const string STACKSMANAGER_HEADER_SINGLE = "Manage stack: {0}";
    public const string STACKSMANAGER_LOG_DEBUGCREATED = "Debug cards created.";
    public const string STACKSMANAGER_PROMPT_DELETESTACK = "Do you want to delete this stack and all its cards?";
    public const string STACKSMANAGER_LOG_STACKDELETED = "Stack deleted.";

    public const string USERINPUT_DATETIMEERROR = "Couldn't parse DateTime. Use provided template.";
    public const string USERINPUT_DATEERROR = "Couldn't parse Date. Use provided template.";
    public const string USERINPUT_OLDERDATEERROR = "Cannot accept dates older than today.";
    public const string USERINPUT_LONGERROR = "Couldn't parse number. Try again.";
}