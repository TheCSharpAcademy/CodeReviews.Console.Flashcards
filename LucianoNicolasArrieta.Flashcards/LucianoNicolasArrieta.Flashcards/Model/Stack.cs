namespace LucianoNicolasArrieta.Flashcards.Model
{
    public class Stack
    {
        public int Id { get; set; }
        public string Subject { get; set; } 
        private List<Flashcard> Flashcards { get; set; }

        public Stack(string subject)
        {
            Subject = subject;
        }
    }
}
