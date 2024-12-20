using FlashCards.Data;
using FlashCards.Models;
using FlashCards.View;
using Spectre.Console;

namespace FlashCards.Services
{
    internal class StudySessionService
    {
        private readonly DataContext _context;
        private readonly FlashcardService _flashcardService;
        private readonly StackService _stackService;

        public StudySessionService(DataContext context, FlashcardService flashcardService, StackService stackService)
        {
            _context = context;
            _flashcardService = flashcardService;
            _stackService = stackService;
        }

        public void StartSession(Models.Stack stack)
        {
            StudySession studySession = new StudySession();
            studySession.StackId = stack.Id;
            studySession.Stack = stack;
            studySession.StartDate = DateTime.Now;

            _context.StudySessions.Add(studySession);
            _context.SaveChanges();

            ActiveSession(studySession);

            EndSession(studySession);
        }

        public void ActiveSession(StudySession studySession)
        {
            while (true)
            {
                bool nextCardExists = NextCard(studySession.StackId);
                if (!nextCardExists) break;

                studySession.Score++;

                bool isNext = UserInterface.StudySessionManageMenu();
                if (!isNext) break;
            }
        }

        public void EndSession(StudySession studySession)
        {
            StudySession oldSession = _context.StudySessions.Where(ss => ss.Id == studySession.Id).FirstOrDefault();
            oldSession.EndDate = DateTime.Now;

            _context.SaveChanges();
        }

        public bool NextCard(int stackId)
        {
            Flashcard nextFlashcard = _flashcardService.GetNextFlashcardToReview(stackId);
            if (nextFlashcard == null) return false;

            UserInterface.ShowFlashcard(nextFlashcard);

            int reviewBreak = (int) UserInterface.ChooseNextShowTime(nextFlashcard);

            nextFlashcard.ReviewBreakInSeconds = reviewBreak;
            nextFlashcard.LastTimeReviewed = DateTime.Now;
            nextFlashcard.NextTimeToReview = DateTime.Now.AddSeconds(reviewBreak);

            _flashcardService.Update(nextFlashcard);
            return true;
        }

        public List<StudySession>? GetAll() {
            return _context.StudySessions.ToList();
        }

        public Table GetReport(Table table)
        {
            List<StudySession> studySessions = GetAll();
            foreach (StudySession s in studySessions)
            {
                TimeSpan totalTime = (s.EndDate - s.StartDate);

                Models.Stack sessionStack = _stackService.GetById(s.StackId);

                table.AddRow(s.Id.ToString(), sessionStack.Name, s.StartDate.ToString(),s.EndDate.ToString(), $"{totalTime.Hours}:{totalTime.Minutes}:{totalTime.Seconds}", s.Score.ToString());
            }

            return table;
        }
    }
}
