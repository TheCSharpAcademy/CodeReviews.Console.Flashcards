namespace SettingsLibrary;

public class Settings
{
    public string connectionString = "Data Source = UHLAHLAH; Initial Catalog = FlashCards; Integrated Security = True; Encrypt=True;TrustServerCertificate=True";
    public string cardsTableName = "Cards";
    public string stacksTableName = "Stacks";
    public string studySessionsTableName = "Seshs";
    public int stackNameCharLimit = 30;
    public int cardNameCharLimit = 30;
    public int cardPromptCharLimit = 100;
    public int cardAnswerCharLimit = 100;

    public Settings() { }
}
