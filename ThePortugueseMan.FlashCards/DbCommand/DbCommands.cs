using SettingsLibrary;

namespace DbCommandsLibrary;

public class DbCommands
{
    public Initialization Initialize;
    public Insertion Insert;
    public Returning Return;
    public Deletion Delete;
    public Updating Update;
    public Checking Check;

    public string cardsTableName, stacksTableName, studySessionsTableName, connectionString;
    public int stackNameLimit, cardNameLimit, cardPromptLimit, cardAnswerLimit;

    public DbCommands()
    {
        Settings settings = new();

        this.connectionString = settings.connectionString;
        this.cardsTableName = settings.cardsTableName;
        this.stacksTableName = settings.stacksTableName;
        this.studySessionsTableName = settings.studySessionsTableName;
        this.stackNameLimit = settings.stackNameCharLimit;
        this.cardNameLimit = settings.cardNameCharLimit;
        this.cardPromptLimit = settings.cardPromptCharLimit;
        this.cardAnswerLimit = settings.cardAnswerCharLimit;

        Initialize = new Initialization
            (this.connectionString, this.cardsTableName, this.stacksTableName, this.studySessionsTableName,
            this.stackNameLimit, this.cardNameLimit, this.cardPromptLimit, this.cardAnswerLimit);
        Insert = new Insertion(this.connectionString, this.cardsTableName, this.stacksTableName, this.studySessionsTableName);
        Return = new Returning(this.connectionString, this.cardsTableName, this.stacksTableName, this.studySessionsTableName);
        Delete = new Deletion(this.connectionString, this.cardsTableName, this.stacksTableName, this.studySessionsTableName);
        Update = new Updating(this.connectionString, this.cardsTableName, this.stacksTableName);
        Check = new Checking(this.connectionString, this.cardsTableName, this.stacksTableName);
    }
}
