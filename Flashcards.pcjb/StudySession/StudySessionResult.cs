namespace Flashcards;

class StudySessionResult
{
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public int ScorePercent
    {
        get
        {
            if (TotalQuestions == 0)
            {
                return 0;
            }
            double scorePercent = 100 * CorrectAnswers / TotalQuestions;
            return (int) Math.Round(scorePercent);
        }
    }
}