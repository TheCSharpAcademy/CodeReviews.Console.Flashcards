using Dapper;
using Flashcards.Dejmenek.DataAccess.Interfaces;
using Flashcards.Dejmenek.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Flashcards.Dejmenek.DataAccess.Repositories;

public class StudySessionsRepository : IStudySessionsRepository
{
    private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["LocalDbConnection"].ConnectionString;

    public void AddStudySession(int stackId, DateTime date, int score)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"INSERT INTO StudySessions (StackId, Date, Score) VALUES
                               (@StackId, @Date, @Score)";

            connection.Execute(sql, new
            {
                StackId = stackId,
                Date = date,
                Score = score
            });
        }
    }

    public IEnumerable<StudySession> GetAllStudySessions()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"SELECT * FROM StudySessions";

            return connection.Query<StudySession>(sql);
        }
    }

    public IEnumerable<MonthlyStudySessionsNumberData> GetMonthlyStudySessionReport(string year)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"SELECT
                                Name AS StackName,
                                ISNULL([January], 0) AS JanuaryNumber,
                                ISNULL([February], 0) AS FebruaryNumber,
                                ISNULL([March], 0) AS MarchNumber,
                                ISNULL([April], 0) AS AprilNumber,
                                ISNULL([May], 0) AS MayNumber,
                                ISNULL([June], 0) AS JuneNumber,
                                ISNULL([July], 0) AS JulyNumber,
                                ISNULL([August], 0) AS AugustNumber,
                                ISNULL([September], 0) AS SeptemberNumber,
                                ISNULL([October], 0) AS OctoberNumber,
                                ISNULL([November], 0) AS NovemberNumber,
                                ISNULL([December], 0) AS DecemberNumber
                               FROM (
                                SELECT Name, DATENAME(month, Date) AS month, st.Id FROM StudySessions s
                                JOIN Stacks st ON s.StackId = st.Id WHERE YEAR(Date) = @Year
                               ) t
                               PIVOT (
                                 COUNT(t.Id)
                                 FOR month IN (
                                     [January], [February], [March],
                                     [April], [May], [June], [July],
                                     [August], [September], [October],
                                     [November], [December]
                                 )
                               ) AS pivot_table";

            return connection.Query<MonthlyStudySessionsNumberData>(sql, new
            {
                Year = year
            });
        }
    }

    public IEnumerable<MonthlyStudySessionsAverageScoreData> GetMonthlyStudySessionAverageScoreReport(string year)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"SELECT 
                                Name AS StackName,
                                ISNULL([January], 0) AS JanuaryAverageScore,
                                ISNULL([February], 0) AS FebruaryAverageScore,
                                ISNULL([March], 0) AS MarchAverageScore,
                                ISNULL([April], 0) AS AprilAverageScore,
                                ISNULL([May], 0) AS MayAverageScore,
                                ISNULL([June], 0) AS JuneAverageScore,
                                ISNULL([July], 0) AS JulyAverageScore,
                                ISNULL([August], 0) AS AugustAverageScore,
                                ISNULL([September], 0) AS SeptemberAverageScore,
                                ISNULL([October], 0) AS OctoberAverageScore,
                                ISNULL([November], 0) AS NovemberAverageScore,
                                ISNULL([December], 0) AS DecemberAverageScore
                               FROM (
                                SELECT Name, DATENAME(month, Date) AS month, ISNULL(Score, 0) as Score FROM StudySessions s
                                JOIN Stacks st ON s.StackId = st.Id WHERE Year(Date) = @Year
                               ) t
                               PIVOT (
                                AVG(t.Score)
                                FOR month IN (
                                   [January], [February], [March],
                                   [April], [May], [June], [July],
                                   [August], [September], [October],
                                   [November], [December]
                                )
                               ) AS pivot_table";

            return connection.Query<MonthlyStudySessionsAverageScoreData>(sql, new
            {
                Year = year
            });
        }
    }

    public bool HasStudySession()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"IF EXISTS (
                                  SELECT 1 FROM StudySessions
                               )
                               BEGIN
                                  SELECT 1;
                               END
                               ELSE
                               BEGIN
                                  SELECT 0;
                               END;";

            return connection.QuerySingle<bool>(sql);
        }
    }
}
