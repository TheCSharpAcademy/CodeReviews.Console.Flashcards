namespace Flashcards.Models
{
    internal class StudySessionDto
    {
        public string StackName { get; set; }
        public DateTime Date { get; set; }
        public int Questions { get; set; }
        public int CorrectAnswers { get; set; }
        public int Percentage { get; set; }
        public TimeSpan Time { get; set; }
    }
}
