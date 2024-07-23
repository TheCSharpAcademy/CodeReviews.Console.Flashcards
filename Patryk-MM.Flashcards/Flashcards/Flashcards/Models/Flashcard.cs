using System.ComponentModel.DataAnnotations.Schema;

namespace Flashcards.Models {
    public class Flashcard : BaseEntity {
        public int StackId { get; set; }
        public Stack Stack { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public override string ToString() {
            return $"Question: {Question}, Answer: {Answer}";
        }
    }
}
