using Flashcards.Domain.Entities;


namespace Flashcards.Domain.Interfaces
{

    public interface IStudySessionRepository
    {
        void Add(StudySession session);
        IEnumerable<StudySession> GetByStackId(int stackId);

        void DeleteByStackId(int stackId);
    }
}