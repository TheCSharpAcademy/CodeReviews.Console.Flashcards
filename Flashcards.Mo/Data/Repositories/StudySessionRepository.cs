using Flashcards.Domain.Entities;
using Flashcards.Domain.Interfaces;
using Flashcards.Data.Context;


namespace Flashcards.Data.Repositories
{
    public class StudySessionRepository : IStudySessionRepository
    {
        private readonly ApplicationDbContext _context;

        public StudySessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public void Add(StudySession studySession)
        {
            _context.StudySessions.Add(studySession);
            _context.SaveChanges();
        }

        
        public IEnumerable<StudySession> GetByStackId(int stackId)
        {
            return _context.StudySessions
                           .Where(ss => ss.StackId == stackId)
                           .ToList();
        }

        
        public StudySession GetById(int studySessionId)
        {
            var session = _context.StudySessions.Find(studySessionId);
            if (session == null)
            {
                throw new KeyNotFoundException($"Study session with ID {studySessionId} was not found.");
            }
            return session;
        }

        public void DeleteByStackId(int stackId)
        {
            var studySessions = GetByStackId(stackId);
            _context.StudySessions.RemoveRange(studySessions);
            _context.SaveChanges();
        }

    }
}
