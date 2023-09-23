namespace Flashcards
{
    internal class Flashcard
    {
        public int Id { get; set; }
        public int StackId { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public static int Count { get; private set; } = 1;
        public FlashcardDTO DTO { get; private set; }
        public FlashcardQuestionDTO QuestionDTO { get; private set; }

        public Flashcard(int stackId, string front, string back)
        {
            Id = Count++;
            StackId = stackId;
            Front = front;
            Back = back;
            DTO = new FlashcardDTO(front,back);
            QuestionDTO = new FlashcardQuestionDTO(front);
        }
        public Flashcard(int id, int stackId, string front, string back)
        {
            Id = id;
            StackId = stackId;
            Front = front;
            Back = back;
            DTO = new FlashcardDTO(front, back);
            QuestionDTO = new FlashcardQuestionDTO(front);
            Count++;
        }
        public static void DownCount() => Count--;

    }

    internal class FlashcardDTO
    {
        public FlashcardDTO(string front, string back)
        {
            Front = front;
            Back = back;
        }

        public string Front { get; set; }
        public string Back { get; set; }
    }

    internal class FlashcardQuestionDTO
    {
        public FlashcardQuestionDTO(string front)
        {
            Front = front;
        }

        public string Front { get; set; }
    }
}
