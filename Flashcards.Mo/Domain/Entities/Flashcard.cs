namespace Flashcards.Domain.Entities
{
    public class Flashcard
    {
        public int Id { get; set; }             
        public int StackId { get; set; }        
        public string Question { get; set; } = string.Empty; 
        public string Answer { get; set; } = string.Empty;

        public Stack? Stack { get; set; }       
        public int DisplayOrder { get; set; } 
    }
}
