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

    public const string STACKSMANAGER_HEADER_ADDCARD = "Create cards: {0}";
    public const string STACKSMANAGER_TOOLTIP_ADDCARD = "Enter '.' to leave";
    public const string STACKSMANAGER_PROMPT_ADDCARD_FRONT = "Enter new card front:";
    public const string STACKSMANAGER_PROMPT_ADDCARD_BACK = "Enter new card back:";
    public const string STACKSMANAGER_LOG_CARDADDED = "Added new card: {0}";

    public const string STACKSMANAGER_HEADER_DELETECARD = "Delete cards: {0}";
    public const string STACKSMANAGER_LOG_STACKEMPTY = "There are no cards in this stack.";
    public const string STACKSMANAGER_PROMPT_DELETECARD = "Select a card to delete";
    public const string STACKSMANAGER_LOG_CARDDELETED = "Card deleted.";

    public const string USERINPUT_DATETIMEERROR = "Couldn't parse DateTime. Use provided template.";
    public const string USERINPUT_DATEERROR = "Couldn't parse Date. Use provided template.";
    public const string USERINPUT_OLDERDATEERROR = "Cannot accept dates older than today.";
    public const string USERINPUT_LONGERROR = "Couldn't parse number. Try again.";
}