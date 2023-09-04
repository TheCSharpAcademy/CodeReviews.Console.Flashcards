namespace Flashcards.w0lvesvvv.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public int CardStackId { get; set; }
        public string CardQuestion { get; set; } = string.Empty;
        public string CardAnswer { get; set; } = string.Empty;
    }
}
