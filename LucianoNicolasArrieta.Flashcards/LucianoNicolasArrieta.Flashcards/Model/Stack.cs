namespace LucianoNicolasArrieta.Flashcards.Model
{
    public class Stack
    {
        public string Subject { get; set; } 
        private List<Flashcard> Flashcards { get; set; }

        public Stack(string subject)
        {
            while (String.IsNullOrEmpty(subject))
            {
                Console.WriteLine("The subject can't be empty. Try again.");
                subject = Console.ReadLine();
            }
            Subject = subject;
        }
    }
}
