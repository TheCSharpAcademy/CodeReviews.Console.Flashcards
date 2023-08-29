namespace FlashCards.Ramseis
{
    internal class Options
    {
        static public string DatabaseName { get; set; } = "Flashcards";
        static public string connectionString = @"Data Source = (localdb)\" + DatabaseName;
    }
}
