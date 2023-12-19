namespace Flashcards;

class StudySession(Stacks stack, DateOnly date, float score, int studySessionID = 0)
{
    public Stacks Stack = stack;
    public DateOnly Date = date;
    public float Score = score;
    public int StudySessionID = studySessionID;
}