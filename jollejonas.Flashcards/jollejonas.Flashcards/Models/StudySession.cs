using System.ComponentModel.DataAnnotations.Schema;
namespace jollejonas.Flashcards.Models
{
    public class StudySession
    {
        public int Id { get; set; }
        [ForeignKey("CardStack")]
        public int CardStackId { get; set; }
        public List<Card> Cards { get; set; }
        public DateTime EndTime { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public CardStack CardStack { get; set; }
    }
}
