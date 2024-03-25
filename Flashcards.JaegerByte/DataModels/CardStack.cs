namespace Flashcards.JaegerByte.DataModels
{
    internal class CardStack
    {
        public int StackID { get; set; }
        public string Title { get; set; }
        public List<Flashcard> Flashcards { get; set; }

        public CardStack(int stackID, string title)
        {
            StackID = stackID;
            Title = title;
        }

        public CardStack(string title)
        {
            Title = title;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
