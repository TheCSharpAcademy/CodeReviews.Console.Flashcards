namespace FlashCards.DTOs
{
    internal class StudySessionDTO
    {
        internal int Id { get; set; }
        internal int DeckId { get; set; }
        internal string DeckName { get; set; }
        internal DateTime SessionDate { get; set; }
        internal int Score { get; set; }
        internal int CardsStudied { get; set; }

        internal StudySessionDTO() 
        {
            DeckName = string.Empty;
        }

        internal StudySessionDTO(int id, int deckId, string deckName, DateTime sessionDate, int score, int cardsStudied)
        {
            Id = id;
            DeckId = deckId;
            DeckName = deckName;
            SessionDate = sessionDate;
            Score = score;
            CardsStudied = cardsStudied;
        }

        public override string ToString()
        {
            return $"{SessionDate.ToString("MM-dd-yyyy hh:mm tt")} / {DeckName} / Score: {Score} out of {CardsStudied}";
        }
    }
}
