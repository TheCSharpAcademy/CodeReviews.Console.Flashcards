namespace Flashcards.Models
{
    internal class StudySession
    {
        public int ID { get; set; }

        public DateTime SessionDate { get; set; }

        public string StackName { get; set; }
        public int Score { get; set; }

        public string StackID { get; set; }
    }
}
