
namespace FlashCards
{
    internal interface IStudySessionServiceUi : IUserInterface
    {
        string DateFormat { get; }

        ConsoleKey GetKeyFromUser();
        void PrintAllSessions(List<CardStack> stacks, List<StudySession> sessions);
        void PrintQuestion(string stack, FlashCardDto card);
        void PrintReportForStack(int year, string stackName, ReportObject sessionCount, ReportObject sessionTotalScore, ReportObject sessionScoreAvg);
        void PrintResult(StudySession session, int numberOfRounds);
        int ValidateAnswer(FlashCardDto card, string answer);
    }
}