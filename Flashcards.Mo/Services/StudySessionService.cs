using Flashcards.Domain.Interfaces;
using Flashcards.Domain.DTO;
using Flashcards.Domain.Entities;
using System.Collections.Generic;

namespace Flashcards.Services
{
    public class StudySessionService
    {
        private readonly IStudySessionRepository _studySessionRepository;

        public StudySessionService(IStudySessionRepository studySessionRepository)
        {
            _studySessionRepository = studySessionRepository;
        }

        public void RecordStudySession(int stackId, int score)
        {
            var studySession = new StudySession
            {
                StackId = stackId,
                Date = DateTime.UtcNow ,
                Score = score
            };
            _studySessionRepository.Add(studySession);
        }

        public IEnumerable<StudySessionDto> GetStudySessionsForStack(int stackId)
        {
            var sessions = _studySessionRepository.GetByStackId(stackId)
                                                  .Select(ss => new StudySessionDto
                                                  {
                                                      Date = ss.Date,
                                                      Score = ss.Score
                                                  });
            return sessions;
        }
    }
}
