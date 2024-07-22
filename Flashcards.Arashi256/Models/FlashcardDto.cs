namespace Flashcards.Arashi256.Models
{
    internal class FlashcardDto
    {
        public int? DisplayId = null;
        public int? Id = null;
        public int StackId;
        public string Subject;
        public string Front = string.Empty;
        public string Back = string.Empty;
    }
}
