using Flashcards.Eddyfadeev.Extensions;
using Flashcards.Eddyfadeev.Interfaces.Database;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Models.Dto;
using Flashcards.Eddyfadeev.Models.Entity;

namespace Flashcards.Eddyfadeev.Repositories;

/// <summary>
/// Represents a repository for managing study sessions.
/// </summary>
internal class StudySessionsRepository : IStudySessionsRepository
{
    private readonly IDatabaseManager _databaseManager;
    
    public StudySessionsRepository(IDatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// Inserts a study session into the StudySessions table.
    /// </summary>
    /// <param name="entity">The study session entity to insert.</param>
    /// <returns>The number of rows affected by the insertion operation.</returns>
    public int Insert(IDbEntity<IStudySession> entity)
    {
        const string query = 
            """
                INSERT INTO StudySessions (Questions, CorrectAnswers, StackId, Time, Date) 
                VALUES (@Questions, @CorrectAnswers, @StackId, @Time, @Date);
            """;
        
        var studySession = entity.MapToDto();
        
        return _databaseManager.InsertEntity(query, studySession);
    }

    /// <summary>
    /// Retrieves all study sessions.
    /// </summary>
    /// <returns>The list of study sessions.</returns>
    public IEnumerable<IStudySession> GetAll()
    {
        const string query =
            """
                SELECT 
                    s.Name as StackName,
                    ss.Date,
                    ss.Questions,
                    ss.CorrectAnswers,
                    ss.Percentage,
                    ss.Time
                FROM
                    StudySessions ss
                INNER JOIN 
                    Stacks s ON ss.StackId = s.Id;
            """;

        IEnumerable<IStudySession> studySessions = _databaseManager.GetAllEntities<StudySessionDto>(query).ToList();
        
        return studySessions.Select(studySession => studySession.ToEntity());
    }

    /// <summary>
    /// Retrieves a collection of study sessions associated with a specific stack.
    /// </summary>
    /// <param name="stack">The stack entity to retrieve study sessions for.</param>
    /// <returns>A collection of study sessions associated with the specified stack.</returns>
    public IEnumerable<IStudySession> GetByStackId(IDbEntity<IStack> stack)
    {
        const string query =
            """
                SELECT 
                    ss.Date,
                    ss.Questions,
                    ss.CorrectAnswers,
                    ss.Percentage,
                    ss.Time,
                    s.Name as StackName
                FROM
                    StudySessions ss
                INNER JOIN 
                    Stacks s ON ss.StackId = s.Id
                WHERE
                    ss.StackId = @Id;
            """;
        
        var stackDto = stack.MapToDto();

        IEnumerable<IStudySession> studySessions = _databaseManager.GetAllEntities<StudySessionDto>(query, stackDto);
        
        return studySessions.Select(studySession => studySession.ToEntity());
    }

    /// <summary>
    /// Retrieves the average number of monthly study sessions for each stack in the specified year.
    /// </summary>
    /// <param name="year">The year for which to retrieve the average.</param>
    /// <returns>A collection of stack monthly sessions containing the stack name and the average session count for each month.</returns>
    public IEnumerable<IStackMonthlySessions> GetAverageYearly(IYear year)
    {
        const string query =
            """
                SELECT
                    StackName,
                    [January], [February], [March], 
                    [April], [May], [June], [July], 
                    [August], [September], [October], 
                    [November], [December]
                FROM
                    (SELECT
                        s.Name AS StackName,
                        DATENAME(MONTH, ss.Date) AS MonthName,
                        COUNT(*) AS SessionCount
                 FROM
                    StudySessions ss
                 JOIN
                    Stacks s ON ss.StackId = s.Id
                 GROUP BY
                    s.Name, DATENAME(MONTH, ss.Date)
                ) AS SourceTable
                PIVOT
                (
                    SUM(SessionCount)
                    FOR MonthName IN (
                        [January], [February], [March], 
                        [April], [May], [June], [July], 
                        [August], [September], [October], 
                        [November], [December]
                    )) AS PivotTable
                ORDER BY
                    StackName;
            """;

        var parameters = new Dictionary<string, object>
        {
            { "@Year", year.ChosenYear }
        };

        IEnumerable<IStackMonthlySessions> studySessions = _databaseManager.GetAllEntities<StackMonthlySessionsDto>(query, parameters).ToList();
        
        return studySessions.Select(studySession => studySession.ToEntity());
    }

    /// <summary>
    /// Returns a collection of distinct years from the StudySessions table.
    /// </summary>
    /// <returns>A collection of IYear objects representing the distinct years.</returns>
    public IEnumerable<IYear> GetYears()
    {
        const string query = "SELECT DISTINCT YEAR(Date) as ChosenYear FROM StudySessions;";

        IEnumerable<IYear> years = _databaseManager.GetAllEntities<Year>(query);
        
        return years.Select(year => year.ToEntity());
    }
}