namespace Flashcards.K_MYR.Models
{
    internal class Session
    {
        public int SessionId { get; set; }

        public int StackId { get; set; }

        public string StackName { get; set; }

        public DateTime Date { get; set; }

        public int Score { get; set; }

        public TimeSpan Duration { get; set; }
    }

    internal class SessionDTO
    {
        public int Row { get; set; }

        public string StackName { get; set; }

        public string Date { get; set; }

        public int Score { get; set; }

        public string Duration { get; set; }

    }
}
