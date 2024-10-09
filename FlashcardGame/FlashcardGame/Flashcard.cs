namespace FlashcardGame
{
    internal class Flashcard
    {
        public int flashcard_Id { get; set; }  
        public string flashCard_Question { get; set; }   
        public string flashcard_Answer { get; set; }   
        public int stack_Id { get; set; }

        public Flashcard(int flashcard_Id, string flashCard_Question, string flashcard_Answer, int stack_Id)
        {
            this.flashcard_Id = flashcard_Id;
            this.flashCard_Question = flashCard_Question;
            this.flashcard_Answer = flashcard_Answer;
            this.stack_Id = stack_Id;
        }

        public Flashcard()
        {
        }
        public FlashcardDTO ConvertToDTO()
        {
            return new FlashcardDTO
            {
                flashcard_Id = this.flashcard_Id,
                flashCard_Question = this.flashCard_Question,
                flashcard_Answer = this.flashcard_Answer
            };
        }
        public override string ToString()
        {
            return string.Format($"{flashcard_Id}. Question: {flashCard_Question} / Answer: {flashcard_Answer}");
        }
    }
    public class FlashcardDTO
    {
        public int flashcard_Id { get; set; }
        public string flashCard_Question { get; set; }
        public string flashcard_Answer { get; set; }
    }
}
