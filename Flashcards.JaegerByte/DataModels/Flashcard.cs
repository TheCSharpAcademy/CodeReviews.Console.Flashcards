namespace Flashcards.JaegerByte.DataModels
{
    internal class Flashcard
    {
        public int FlashcardID { get; set; }
        public int StackID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public Flashcard(int flashcardID, string question, string answer, int stackID)
        {
            FlashcardID = flashcardID;
            StackID = stackID;
            Question = question;
            Answer = answer;
        }

        public Flashcard(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }

        public override string ToString()
        {
            return $"{FlashcardID} - {Question}";
        }
    }
}
