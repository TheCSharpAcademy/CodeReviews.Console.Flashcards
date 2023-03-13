using Microsoft.VisualBasic;
using SettingsLibrary;
using System.Net.Http.Headers;

namespace DbCommandsLibrary;

public class DbCommands
{
    public Initialization Initialize;
    public Insertion Insert;
    public Returning Return;
    public Deletion Delete;
    public Updating Update;



    public string cardsTableName, stacksTableName, connectionString;
    public int stackNameLimit, cardNameLimit, cardPromptLimit, cardAnswerLimit;

    public DbCommands()
    {
        Settings settings = new();

        this.connectionString = settings.connectionString;
        this.cardsTableName = settings.cardsTableName;
        this.stacksTableName = settings.stacksTableName;
        this.stackNameLimit = settings.stackNameCharLimit;
        this.cardNameLimit = settings.cardNameCharLimit;
        this.cardPromptLimit = settings.cardPromptCharLimit;
        this.cardAnswerLimit = settings.cardAnswerCharLimit;

        Initialize = new Initialization(this.connectionString, this.cardsTableName, this.stacksTableName,
            this.stackNameLimit, this.cardNameLimit, this.cardPromptLimit, this.cardAnswerLimit);
        Insert = new Insertion(this.connectionString, this.cardsTableName, this.stacksTableName);
        Return = new Returning(this.connectionString, this.cardsTableName, this.stacksTableName);
        Delete = new Deletion(this.connectionString, this.cardsTableName, this.stacksTableName);
        Update = new Updating(this.connectionString, this.cardsTableName, this.stacksTableName);
    }
}
