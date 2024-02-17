using FlashCards.Cactus.DataModel;

namespace FlashCards.Cactus.Service;

public class StudySessionService
{
    public StudySessionService()
    {
        StudySessions = new List<StudySession>() {
            new StudySession(1, "English", new TimeSpan(1,10,0), 10),
            new StudySession(2, "Algorithm", new TimeSpan(2,0,0), 40)
        };
    }

    public List<StudySession> StudySessions { get; set; }

    public void ShowAllStudySessions()
    {
        List<List<string>> rows = new List<List<string>>();
        StudySessions.ForEach(ss => rows.Add(new List<string>() { ss.StackName, ss.Time.TotalMinutes.ToString(), ss.Score.ToString() }));
        ServiceHelpers.ShowDataRecords(Constants.STUDYSESSION, Constants.STUDYSESSIONS, rows);
    }

    public void StartFromExistingStudySession()
    {
        Console.WriteLine("Start from an existing study session.");
    }

    public void StartNewStudySession()
    {
        Console.WriteLine("Start a new study session.");
    }

    public void DeleteStudySession()
    {
        Console.WriteLine("Delete a study session.");
    }
}

