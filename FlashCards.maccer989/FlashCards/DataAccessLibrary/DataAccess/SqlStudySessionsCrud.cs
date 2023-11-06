using DataAccessLibrary.Models;

namespace DataAccessLibrary.DataAccess
{
    public class SqlStudySessionsCrud
    {
        private readonly string _connectionString;
        private SqlDataAccess db = new SqlDataAccess();
        private List<StudySessionsModel> output;
        public SqlStudySessionsCrud(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<StudySessionsModel> CreateStudySession(StudySessionsModel session)
        {
            List <StudySessionsModel> output;
            string sql = "insert into StudySessions (StackName, StudySessionDate, TotalAnswerCorrect) values (@StackName, @StudySessionDate, @TotalAnswerCorrect);";
            db.SaveData(sql,
                        new { session.StackName, session.StudySessionDate, session.TotalAnswerCorrect },
                        _connectionString);
            sql = "select top 1* from StudySessions order by StudySessionDate desc";
            output = db.LoadData<StudySessionsModel, dynamic>(sql, new { }, _connectionString);
            return output;

        }
        public List<StudySessionsModel> GetAllStudySessions()
        {
            string sql = "select * from StudySessions";
            var checkQuery = db.LoadData<StudySessionsModel, dynamic>(sql, new { }, _connectionString);
            if (checkQuery.Count > 0)
            {
                output = db.LoadData<StudySessionsModel, dynamic>(sql, new { }, _connectionString);
            }
            else
            {
                Console.WriteLine($"There are no Study Sessions.");
            }
            return output;
        }
        public List<FlashCardsModel> GetAllFlashCardsByStackName(int stackName)
        {
            List<FlashCardsModel> output;
            string sql = $"select * from FlashCards where StackName = {stackName};";
            var checkQuery = db.LoadData<FlashCardsModel, dynamic>(sql, new { }, _connectionString);
            if (checkQuery.Count > 0)
            {
                output = db.LoadData<FlashCardsModel, dynamic>(sql, new { }, _connectionString);
            }
            else
            {
                Console.WriteLine($"There are no Flashcards.");
            }
            return checkQuery;
        }
        public bool CheckRecordExists(int recordId)
        {
            bool output = false;
            string sql = $"select StackName from Stacks where StackName = {recordId};";
            var checkQuery = db.LoadData<StacksModel, dynamic>(sql,
                new { Id = recordId },
                _connectionString);

            if (checkQuery.Count > 0)
            {
                Console.WriteLine($"Stack with name {recordId} exists.");
                output = true;
            }
            else
            {
                Console.WriteLine($"Stack with name {recordId} does not exist.");
            }
            return output;
        }
    }
}
