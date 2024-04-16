namespace DatabaseLibrary.Models;

public class StudySession
{
  public int Session_Id { get; set; }
  public DateTime Date { get; set; }
  public int Score { get; set; }
  public int? Stack_Id { get; set; }

  public StudySession() { }

  public StudySession(DateTime date, int? stackId)
  {
    Date = date;
    Score = 0;
    Stack_Id = stackId;
  }
}

public static class StudySessionsExtensions
{
  public static StudySessionDTO ToStudySessionDTO(this StudySession studySession, string stackName)
  {
    return new StudySessionDTO(studySession.Session_Id, studySession.Date, studySession.Score, stackName);
  }
}