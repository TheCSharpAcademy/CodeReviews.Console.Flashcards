using Dapper;
using Flashcards.Arashi256.Classes;
using Flashcards.Arashi256.Models;

namespace Flashcards.Arashi256.Controllers
{
    internal class StudySessionController
    {
        private StudySessionDatabase _studysessionDatabase;
        private StackController _stackController;

        public StudySessionController()
        {
            _studysessionDatabase = new StudySessionDatabase();
            _stackController = new StackController();
        }

        public bool AddStudySession(StudySessionDto dtoStudySession)
        {
            StudySession newStudySession = new StudySession() { Id = dtoStudySession.Id, StackId = dtoStudySession.StackId, TotalCards = dtoStudySession.TotalCards, Score = dtoStudySession.Score, DateStudied = dtoStudySession.DateStudied };
            int rows = _studysessionDatabase.AddNewStudySession(newStudySession);
            return rows > 0 ? true : false;
        }

        public List<StudySessionDto> GetAllStackStudySessionsForDate(int StackId, DateTime d)
        {
            List<StudySessionDto> viewSessions = new List<StudySessionDto>();
            var parameters = new DynamicParameters();
            parameters.Add("@DateStudied", d);
            parameters.Add("@StackId", StackId);
            List<StudySession> sessions = _studysessionDatabase.GetStudySessionResults("SELECT * FROM dbo.studysessions WHERE DateStudied = @DateStudied AND StackId = @StackId ORDER BY DateStudied ASC", parameters);
            for (int i = 0; i < sessions.Count; i++)
            {
                viewSessions.Add(new StudySessionDto() { DisplayId = i + 1, Id = sessions[i].Id, TotalCards = sessions[i].TotalCards, Score = sessions[i].Score, DateStudied = sessions[i].DateStudied, Subject = GetSubjectFromStackID(sessions[i].StackId) });
            }
            return viewSessions;
        }

        public List<StudySessionDto> GetAllStackStudySessionsForStack(int StackId)
        {
            List<StudySessionDto> viewSessions = new List<StudySessionDto>();
            var parameters = new DynamicParameters();
            parameters.Add("@StackId", StackId);
            List<StudySession> sessions = _studysessionDatabase.GetStudySessionResults("SELECT * FROM dbo.studysessions WHERE StackId = @StackId ORDER BY DateStudied ASC", parameters);
            for (int i = 0; i < sessions.Count; i++)
            {
                viewSessions.Add(new StudySessionDto() { DisplayId = i + 1, Id = sessions[i].Id, TotalCards = sessions[i].TotalCards, Score = sessions[i].Score, DateStudied = sessions[i].DateStudied, Subject = GetSubjectFromStackID(sessions[i].StackId) });
            }
            return viewSessions;
        }

        public List<StudySessionDto> GetAllStudySessionsForDate(DateTime d)
        {
            List<StudySessionDto> viewSessions = new List<StudySessionDto>();
            var parameters = new DynamicParameters();
            parameters.Add("@Date", d);
            List<StudySession> sessions = _studysessionDatabase.GetStudySessionResults("SELECT * FROM dbo.studysessions WHERE DateStudied = @DateStudied ORDER BY DateStudied ASC", parameters);
            for (int i = 0; i < sessions.Count; i++)
            {
                viewSessions.Add(new StudySessionDto() { DisplayId = i + 1, Id = sessions[i].Id, TotalCards = sessions[i].TotalCards, Score = sessions[i].Score, DateStudied = sessions[i].DateStudied, Subject = GetSubjectFromStackID(sessions[i].StackId) });
            }
            return viewSessions;
        }

        private string GetSubjectFromStackID(int sid)
        {
            StackDto dtoStack = _stackController.GetStack(sid);
            return dtoStack.Subject;
        }

        public StudySessionReportPerStackDto StudySessionsPerMonthStackReport(int stackid, string year)
        {
            StudySessionReportPerStackDto report = new StudySessionReportPerStackDto();
            var parameters = new DynamicParameters();
            parameters.Add("@StackId", stackid);
            parameters.Add("@Year", year);
            string sql = @"SELECT Subject, [January] AS Jan, [February] AS Feb, [March] AS Mar, [April] AS Apr, [May] AS May,
                        [June] AS Jun, [July] AS Jul, [August] AS Aug, [September] AS Sep, [October] AS Oct, [November] AS Nov, [December] AS Dec
                        FROM (SELECT s.Subject, DATENAME(month, ss.DateStudied) AS MonthName FROM dbo.studysessions ss INNER JOIN  dbo.stacks s ON ss.StackId = s.Id AND s.Id = @StackId 
                        WHERE YEAR(ss.DateStudied) = @Year) AS SourceTable
                        PIVOT (COUNT(MonthName) FOR MonthName IN ([January], [February], [March], [April], [May], [June], [July], [August], [September], [October], [November], [December])) AS PivotTable
                        ORDER BY Subject;";
            report = _studysessionDatabase.GetStudySessionReportForStack(sql, parameters).FirstOrDefault();
            return report;
        }

        public StudySessionReportPerStackDto StudySessionsAveragesStackReport(int stackid, string year)
        {
            StudySessionReportPerStackDto report = new StudySessionReportPerStackDto();
            var parameters = new DynamicParameters();
            parameters.Add("@StackId", stackid);
            parameters.Add("@Year", year);
            string sql = @"SELECT Subject, [January] AS Jan, [February] AS Feb, [March] AS Mar, [April] AS Apr, [May] AS May, [June] AS Jun, [July] AS Jul, 
                        [August] AS Aug,[September] AS Sep, [October] AS Oct, [November] AS Nov, [December] AS Dec
                        FROM (SELECT s.Subject, DATENAME(month, ss.DateStudied) AS MonthName, ss.Score FROM dbo.studysessions ss INNER JOIN dbo.stacks s ON ss.StackId = s.Id AND s.Id = @StackId
                        WHERE YEAR(ss.DateStudied) = @Year) AS SourceTable
                        PIVOT (AVG(Score) FOR MonthName IN ([January], [February], [March], [April], [May], [June], [July], [August], [September], [October], [November], [December])) AS PivotTable
                        ORDER BY Subject;";
            report = _studysessionDatabase.GetStudySessionReportForStack(sql, parameters).FirstOrDefault();
            return report;
        }
    }
}