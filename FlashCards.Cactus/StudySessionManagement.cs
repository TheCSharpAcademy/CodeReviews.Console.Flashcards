
using FlashCards.Cactus.DataModel;

namespace FlashCards.Cactus;

public class StudySessionManagement
{
    public List<StudySession> StudySessions { get; set; }

    public void ShowAllStudySessions()
    {
        Console.WriteLine("Show all study sessions.");
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

