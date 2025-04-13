namespace Flashcards.Object_Classes
{
    public class StudySessionDto
    {
        public int StudySessionId { get; set; }
        public string StackName { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public string SessionDate { get; set; }
        public int SessionDuration { get; set; }
    }
}
