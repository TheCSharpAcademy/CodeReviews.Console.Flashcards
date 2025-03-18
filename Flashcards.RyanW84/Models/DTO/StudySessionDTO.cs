namespace Flashcards.RyanW84.Models.DTO
{
    internal class StudySessionDTO
    {
        public string StackName { get; set; }
        public DateTime Date { get; set; }
        public int Questions { get; set; }
        public int CorrectAnswers { get; set; }
        public int Percentage { get; set; }
        public TimeSpan Time { get; set; }
    }
}
