namespace vcesario.Flashcards;

public static class ApplicationTexts
{
    public const string OPTION_RETURN = "Return";
    public const string PROMPT_ACTION = "What do you want to do?";
    public const string PROMPT_REALLYDELETE = "Are you REALLY sure?";
    
    public const string TABLE_DATE = "Date";
    public const string TABLE_STACKNAME = "Stack name";
    public const string TABLE_SCORE = "Score";
    public const string TABLE_ID = "Id";
    public const string TABLE_CARDFRONT = "Card front";
    public const string TABLE_CARDBACK = "Card back";

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
    public const string TOOLTIP_LEAVE = "Enter '.' to leave";
    public const string STACKSMANAGER_PROMPT_ADDCARD_FRONT = "Enter new card front:";
    public const string STACKSMANAGER_PROMPT_ADDCARD_BACK = "Enter new card back:";
    public const string STACKSMANAGER_LOG_CARDADDED = "Added new card: {0}";

    public const string STACKSMANAGER_HEADER_DELETECARD = "Delete cards: {0}";
    public const string STACKSMANAGER_LOG_STACKEMPTY = "There are no cards in this stack.";
    public const string STACKSMANAGER_PROMPT_DELETECARD = "Select a card to delete";
    public const string STACKSMANAGER_PROMPT_RENAMESTACK = "Enter new stack name:";
    public const string STACKSMANAGER_LOG_CARDDELETED = "Card deleted.";
    public const string STACKSMANAGER_LOG_STACKUPDATED = "Stack name updated.";

    public const string STUDYAREA_HEADER = "Study area";
    public const string STUDYAREA_HEADER_SESSION = "Study session: {0}";
    public const string STUDYAREA_ROUND = "Round {0} / {1}";
    public const string STUDYAREA_LOG_CORRECT = "Correct!";
    public const string STUDYAREA_LOG_INCORRECT = "Incorrect.";
    public const string STUDYAREA_LOG_FINALSCORE = "Final score: {0} / {1}";
    public const string STUDYAREA_LOG_ACED = "ACED!";
    public const string STUDYAREA_HEADER_VIEWLASTSESSIONS = "View last sessions";
    public const string STUDYAREA_HEADER_VIEWANNUALREPORT = "View annual report";
    public const string STUDYAREA_PROMPT_YEAR = "Enter a year:";
    public const string STUDYAREA_HEADER_STUDYSESSIONSPERMONTH = "STUDY SESSIONS PER MONTH";
    public const string STUDYAREA_HEADER_AVERAGESCORE = "AVERAGE SCORE PER MONTH";
    public const string STUDYAREA_LOG_DEBUGCREATED = "Debug sessions created.";

    public const string USERINPUT_DATETIMEERROR = "Couldn't parse DateTime. Use provided template.";
    public const string USERINPUT_DATEERROR = "Couldn't parse Date. Use provided template.";
    public const string USERINPUT_OLDERDATEERROR = "Cannot accept dates older than today.";
    public const string USERINPUT_LONGERROR = "Couldn't parse number.";
    public const string USERINPUT_EXISTINGSTACK = "There's already a stack with that name.";
}