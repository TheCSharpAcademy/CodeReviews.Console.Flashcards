namespace Flashcards
{
    internal class Flashcard
    {
        public int Id { get; set; }
        public int StackId { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public static int Count { get; private set; } = 1;
        public FlashcardDto Dto { get; private set; }
        public FlashcardQuestionDto QuestionDto { get; private set; }

        public Flashcard(int stackId, string front, string back)
        {
            Id = Count++;
            StackId = stackId;
            Front = front;
            Back = back;
            Dto = new FlashcardDto(front,back);
            QuestionDto = new FlashcardQuestionDto(front);
        }
        public Flashcard(int id, int stackId, string front, string back)
        {
            Id = id;
            StackId = stackId;
            Front = front;
            Back = back;
            Dto = new FlashcardDto(front, back);
            QuestionDto = new FlashcardQuestionDto(front);
            Count++;
        }
        public static void DownCount() => Count--;

    }

    internal class FlashcardDto
    {
        public FlashcardDto(string front, string back)
        {
            Front = front;
            Back = back;
        }

        public string Front { get; set; }
        public string Back { get; set; }
    }

    internal class FlashcardQuestionDto
    {
        public FlashcardQuestionDto(string front)
        {
            Front = front;
        }

        public string Front { get; set; }
    }
}
