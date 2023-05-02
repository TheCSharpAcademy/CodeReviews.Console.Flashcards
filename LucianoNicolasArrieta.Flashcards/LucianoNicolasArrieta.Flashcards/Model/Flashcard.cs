namespace LucianoNicolasArrieta.Flashcards.Model
{
    public class Flashcard
    {
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
