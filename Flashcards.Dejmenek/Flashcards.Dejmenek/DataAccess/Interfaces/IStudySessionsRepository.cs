using Flashcards.Dejmenek.Models;

namespace Flashcards.Dejmenek.DataAccess.Interfaces;

internal interface IStudySessionsRepository
{
    IEnumerable<MonthlyStudySessionsNumberData> GetMonthlyStudySessionReport(string year);
    IEnumerable<MonthlyStudySessionsAverageScoreData> GetMonthlyStudySessionAverageScoreReport(string year);
    void AddStudySession(int stackId, DateTime date, int score);
    IEnumerable<StudySession> GetAllStudySessions();
    bool HasStudySession();
}
