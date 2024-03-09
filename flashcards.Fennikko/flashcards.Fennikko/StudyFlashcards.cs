using System.Configuration;
using flashcards.Fennikko.Models;

namespace flashcards.Fennikko;

public class StudyFlashcards
{
    public static readonly string? ConnectionString = ConfigurationManager.AppSettings.Get("connectionString");

    public static void NewStudySession()
    {

    }

    public static void GetStudySessions()
    {

    }

    public static void TableCreation(IEnumerable<StudySessions> sessions, string stackName)
    {

    }
}