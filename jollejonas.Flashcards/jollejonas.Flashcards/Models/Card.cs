using System.ComponentModel.DataAnnotations.Schema;

namespace jollejonas.Flashcards.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        [ForeignKey("CardStack")]
        public int CardStackId { get; set; }

        public CardStack CardStack { get; set; }
    }
}
