
namespace FlashCards
{
    internal interface IStudySessionService
    {
        IStudySessionRepository StudySessionRepository { get; set; }
        IStudySessionServiceUi UserInterface { get; set; }

        void NewStudySession(CardStack stack, List<FlashCardDto> cards);
        bool PrepareRepository(List<CardStack> stacks, List<StudySession> defaultData);
        void PrintAllSessions(List<CardStack> stacks);
        void PrintReport(List<CardStack> stacks);
    }
}