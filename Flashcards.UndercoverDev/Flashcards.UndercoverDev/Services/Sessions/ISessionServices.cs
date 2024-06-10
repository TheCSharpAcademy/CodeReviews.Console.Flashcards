using Flashcards.UndercoverDev.Models;

namespace Flashcards.UndercoverDev.Services.Session
{
    public interface ISessionServices
    {
        void StartSession();
        void ViewSession();
        void DeleteSession(int stackId);
        List<YearlyStudySessionReport> GenerateYearlyReport();
        void DisplayYearlyReport();
    }
}