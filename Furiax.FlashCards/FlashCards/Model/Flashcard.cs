namespace FlashCards.Model
{
	internal class Flashcard
	{
        public int FlashcardId { get; set; }
        public string FrontText { get; set; }
        public string BackText { get; set; }
        public int StackId { get; set; }
    }
}
