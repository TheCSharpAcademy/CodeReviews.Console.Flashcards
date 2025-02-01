using Dapper;
using Flashcards.FunRunRushFlush.Data.Interfaces;
using Flashcards.FunRunRushFlush.Data.Model;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Flashcards.FunRunRushFlush.Data;

public class StudySessionDataAccess : IStudySessionDataAccess
{
    private readonly IDbConnection _connection;

    public StudySessionDataAccess(IDbConnection connection)
    {
        _connection = connection;
    }

    // In this approach I try to combine the best of both worlds:
    // static queries, defined at compile time for better performance, 
    // while keeping them parameterized to maintain flexibility when the model changes.
    //(I didnt like re-interpolating my strings every time the query was called in my previous Acadamy-Projects)


    private static readonly string QueryGetAllStudySessionsWithStackName = $"""
                SELECT 
                    ss.Id, 
                    ss.StackId, 
                    ss.UsedFlashcards, 
                    ss.Date, 
                    st.Name AS StackName
                FROM {StudySessionTable.TableName} ss
                INNER JOIN {StackTable.TableName} st ON ss.StackId = st.Id;
                """;

    public List<StudySession> GetAllStudySessions()
    {
        List<StudySession> sessions = _connection.Query<StudySession>(QueryGetAllStudySessionsWithStackName).ToList();
        return sessions;
    }


    private static readonly string QueryCreateStudySession = $"""
                INSERT INTO {StudySessionTable.TableName} 
                    ({StudySessionTable.StackId},
                    {StudySessionTable.UsedFlashcards},
                    {StudySessionTable.Date})
                VALUES (@{StudySessionTable.StackId},
                        @{StudySessionTable.UsedFlashcards},
                        @{StudySessionTable.Date});
                """;
    public void CreateStudySession(StudySession sSession)
    {
        _connection.Execute(QueryCreateStudySession, new { sSession.StackId, sSession.UsedFlashcards, sSession.Date });
    }


}
