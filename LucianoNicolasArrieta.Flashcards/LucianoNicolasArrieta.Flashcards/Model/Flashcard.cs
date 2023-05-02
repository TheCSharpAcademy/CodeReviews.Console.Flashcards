namespace LucianoNicolasArrieta.Flashcards.Model
{
    public class Flashcard
    {
        public int Id { get; set; }
        private Stack Stack { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public Flashcard(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }
}
