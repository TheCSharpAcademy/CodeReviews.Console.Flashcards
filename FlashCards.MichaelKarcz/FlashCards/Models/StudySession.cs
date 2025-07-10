namespace FlashCards.Models;
internal class StudySession
{
    internal int Id { get; set; }
    internal int DeckId { get; set; }
    internal string DeckName {  get; set; }
    internal int Score { get; set; }
    internal int CardsStudied {  get; set; }
    internal DateTime SessionDate { get; set; }

    internal StudySession() 
    {
        DeckName = string.Empty;
        SessionDate = DateTime.Now;
    }
}
