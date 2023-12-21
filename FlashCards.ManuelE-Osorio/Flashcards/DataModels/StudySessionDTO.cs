namespace Flashcards;

class StudySessionDTO(StudySession studySession, int studySessionID = 0)
{
    public string StackName = studySession.StackName;
    public string Date = studySession.Date.ToString("yyyy/MM/dd");
    public string Score = (studySession.Score * 100).ToString("0.#");
    public int StudySessionID = studySessionID;
}