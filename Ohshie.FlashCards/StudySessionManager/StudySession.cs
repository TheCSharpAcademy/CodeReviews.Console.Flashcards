namespace Ohshie.FlashCards.StudySessionManager;

public class StudySession
{
    public int Id { get; set; }
    public string Date { get; set; } = string.Empty;
    public string DeckName { get; set; }
    public int FlashcardsSolved { get; set; }
}