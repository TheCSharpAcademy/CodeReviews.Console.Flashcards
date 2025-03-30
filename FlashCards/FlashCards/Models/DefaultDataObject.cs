namespace FlashCards
{
    internal class DefaultDataObject
    {
        public List<CardStack> Stacks { get; set; } = new List<CardStack>();
        public List<FlashCard> FlashCards { get; set; } = new List<FlashCard>();
        public List<StudySession> StudySessions { get; set; } = new List<StudySession>();

    }
}
