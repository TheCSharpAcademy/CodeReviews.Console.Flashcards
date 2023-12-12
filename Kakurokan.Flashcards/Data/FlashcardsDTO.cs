namespace Kakurokan.Flashcards
{
    internal class FlashcardsDTO
    {
        public int Id { get; private set; }
        public string Question { get; private set; }
        public string Answer { get; private set; }
        public FlashcardsDTO(int id, string question, string answer)
        {
            Id = id;
            Question = question;
            Answer = answer;
        }

    }
}
