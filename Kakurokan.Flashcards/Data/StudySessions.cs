using System;

namespace Kakurokan.Flashcards
{
    public class StudySessions
    {
        public int Id { get; private set; }
        public int StackId { get; private set; }

        public int Score;
        public string Date { get; private set; }

        public StudySessions()
        {
            Score = 0;
        }

        public StudySessions(int stackId)
        {
            StackId = stackId;
            Date = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
}
