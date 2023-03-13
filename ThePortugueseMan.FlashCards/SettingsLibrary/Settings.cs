namespace SettingsLibrary;

public class Settings
{
    public string connectionString = "Data Source = UHLAHLAH; Initial Catalog = FlashCards; Integrated Security = True; Encrypt=True;TrustServerCertificate=True";
    public string cardsTableName = "Cards";
    public string stacksTableName = "Stacks";
    public int stackNameCharLimit = 30;
    public int cardNameCharLimit = 30;
    public int cardPromptCharLimit = 100;
    public int cardAnswerCharLimit = 100;

    public Settings() { }

    public string GetConnectionString() { return connectionString; }
    
    public string GetCardsTableName() { return cardsTableName; }

    public string GetStacksTableName() { return stacksTableName; } 

    public int GetCardNameCharLimit() { return cardNameCharLimit; }

    public int GetCardPromptCharLimit() { return cardPromptCharLimit; }

    public int GetCardAnswerCharLimit() { return cardAnswerCharLimit; }
}
