
namespace FlashCards
{
    internal interface IStudySessionRepository
    {
        string ConnectionString { get; }

        void AutoFill(List<CardStack> stacks, List<StudySession> sessions);
        void CreateTable();
        bool DoesTableExist();
        IEnumerable<StudySession>? GetAllRecords();
        ReportObject? GetDataPerMonthInYear(CardStack stack, int year, PivotFunction pivotFunction);
        bool Insert(StudySession entity);
    }
}