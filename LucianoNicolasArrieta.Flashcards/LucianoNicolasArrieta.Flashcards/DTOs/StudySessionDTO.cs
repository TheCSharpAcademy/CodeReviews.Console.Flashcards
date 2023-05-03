namespace LucianoNicolasArrieta.Flashcards.DTOs
{
    public class StudySessionDTO
    {
        public string Date { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }

        public string Subject { get; set; }
    }
}
