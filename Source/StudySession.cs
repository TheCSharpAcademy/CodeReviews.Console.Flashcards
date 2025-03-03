namespace vcesario.Flashcards;

public class StudySession
{
    public int Id => m_Id;
    public int StackId => m_StackId;
    public DateTime Date => m_Date;
    public int Score => m_Score;

    private int m_Id;
    private int m_StackId;
    private DateTime m_Date;
    private int m_Score;

    public StudySession(int Id, int StackId, DateTime Date, int Score)
    {
        m_Id = Id;
        m_StackId = StackId;
        m_Date = Date;
        m_Score = Score;
    }
}