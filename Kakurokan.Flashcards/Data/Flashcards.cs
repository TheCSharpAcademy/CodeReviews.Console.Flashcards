namespace Kakurokan.Flashcards
{
    public class Flashcards
    {
        public int Id { get; private set; }
        public int StackId { get; private set; }
        public string Question { get; private set; }
        public string Answer { get; private set; }
        public Flashcards(int id, int stackId, string question, string answer)
        {
            Id = id;
            StackId = stackId;
            Question = question;
            Answer = answer;
        }

        public Flashcards(int stackId, string question, string answer)
        {
            StackId = stackId;
            Question = question;
            Answer = answer;
        }

        public Flashcards(int id) => Id = id;


        public override string ToString()
        {
            return
            $"Id: {Id}" +
            $"Question: {Question}";
        }
    }
}
