using Flashcards.Models;

namespace Flashcards.DTOs
{
    internal static class DtoMapper
    {
        private static Dictionary<int, int> _FlashcardIDMap = new Dictionary<int, int>();
        private static Dictionary<int, int> _StackIDMap = new Dictionary<int, int>();
        private static Dictionary<int, int> _SessionIDMap = new();

        public static Dictionary<int, int> SessionIDMap
        {
            get { return _SessionIDMap; }
        }
        public static Dictionary<int, int> FlashcardIDMap
        {
            get { return _FlashcardIDMap; }
        }
        public static Dictionary<int, int> StackIDMap
        {
            get { return _StackIDMap; }
        }

        internal static StudySessionDto ToStudySessionDto(StudySession studySession, int dtoID)
        {
            int originalID = studySession.ID;

            _SessionIDMap[dtoID] = originalID;
            return new StudySessionDto { ID = dtoID, SessionDate = studySession.SessionDate, StackName = studySession.StackName, Score = studySession.Score };
        }

        internal static StackDto ToStackDto(StackModel stack, int dtoID)
        {
            int originalID = stack.Id;

            _StackIDMap[dtoID] = originalID;
            return new StackDto { ID = dtoID, Name = stack.Name };
        }

        internal static FlashcardDto ToFlashcardDto(Flashcard flashcard, int dtoID)
        {
            int originalID = Convert.ToInt32(flashcard.ID);

            _FlashcardIDMap[dtoID] = originalID;
            return new FlashcardDto { ID = dtoID, Front = flashcard.Front, Back = flashcard.Back };
        }


    }
}
