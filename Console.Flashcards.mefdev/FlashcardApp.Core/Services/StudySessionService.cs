using FlashcardApp.Core.Repositories.Interfaces;
using FlashcardApp.Core.Services.Interfaces;
using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Services
{
    public class StudySessionService : IStudySessionService
	{
        private readonly IStudySessionRepository _studySessionRepository;

        public StudySessionService(IStudySessionRepository studySessionRepository)
		{
            _studySessionRepository = studySessionRepository;
        }

        public async Task<Result<string>> AddStudySession(StudySession studySession)
        {
            studySession.Id = GenerateRandomID();
            var existingStudySession = await _studySessionRepository.GetStudySessionById(studySession.Id);
            if (existingStudySession != null)
            {
                return Result<string>.Failure("The CodingSession is already exists.");
            }
            await _studySessionRepository.AddStudySession(new StudySession
            {
                Id= studySession.Id,
                stack =studySession.stack,
                CurrentDate = studySession.CurrentDate,
                Score = studySession.Score

            });
            return Result<string>.Success("success");
        }

        public int GenerateRandomID()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        }

        public async Task<Result<IEnumerable<StudySession>>> GetStudySessionsByStackName(string name)
        {
            var studySessions = await _studySessionRepository.GetStudySessionsByStackName(name);
            if (studySessions == null || !studySessions.Any())
            {
                return Result<IEnumerable<StudySession>>.Failure("Notice: No StudySessions found.");
            }
            return Result<IEnumerable<StudySession>>.Success(studySessions);
        }
    }
}