using Flashcards.Models;

namespace Flashcards.DTOs
{
    internal static class DTOMapper
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

        internal static StudySessionDTO toStudySessionDTO(StudySession studySession, int dtoID)
        {
            int originalID = studySession.ID;

            _SessionIDMap[dtoID] = originalID;
            return new StudySessionDTO { ID = dtoID, SessionDate = studySession.SessionDate, StackName = studySession.StackName, Score = studySession.Score };
        }

        internal static StackDTO toStackDTO(StackModel stack, int dtoID)
        {
            int originalID = stack.Id;

            _StackIDMap[dtoID] = originalID;
            return new StackDTO { ID = dtoID, Name = stack.Name };
        }

        internal static FlashcardDTO toFlashcardDTO(Flashcard flashcard, int dtoID)
        {
            int originalID = Convert.ToInt32(flashcard.ID);

            _FlashcardIDMap[dtoID] = originalID;
            return new FlashcardDTO { ID = dtoID, Front = flashcard.Front, Back = flashcard.Back };
        }


    }
}
