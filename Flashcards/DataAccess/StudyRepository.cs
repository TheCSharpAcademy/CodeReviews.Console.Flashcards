using Dapper;

public class StudyRepository
{
    private DatabaseManager _databaseManager;

    public StudyRepository(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void AddStudy(StudySession studySession)
    {

        using (var conn = _databaseManager.GetConnection())
        {
            var query = "INSERT INTO StudySessions (StackId, Date, Score, TotalQuestions) VALUES (@StackId, @Date, @Score, @TotalQuestions)";
            conn.Execute(query, studySession);
        }
    }

    public List<StudySession> GetStudySessions()
    {
        using (var conn = _databaseManager.GetConnection())
        {
            var query = "SELECT * FROM StudySessions";
            return conn.Query<StudySession>(query).ToList();
        }
    }
}
