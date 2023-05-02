namespace LucianoNicolasArrieta.Flashcards.Model
{
    public class Flashcard
    {
        public int Id { get; set; }
        private Stack Stack { get; set; }
        private string Question { get; set; }
        private string Answer { get; set; }

        public Flashcard(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }
}
