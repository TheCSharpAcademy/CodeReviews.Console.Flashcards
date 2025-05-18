using Dapper;
using Flashcards.Models;
using Flashcards.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Repository
{
    public class StudySessionRepository : IStudySessionRepository
    {
        private readonly string? _connectionString;
        public StudySessionRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("defaultConnection");
        }
        public List<StudySessionDTO> GetAllSessions()
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            List<StudySessionDTO> li = conn.Query<StudySessionDTO>("SELECT s.Name AS StackName, ss.StudyDate, ss.Score FROM [dbo].[StudySessions] as ss INNER JOIN [dbo].[Stacks] as s ON ss.StackId = s.Id").ToList();
            return li;
        }

        public void SaveStudySession(StudySession session)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            conn.Execute($"INSERT INTO StudySessions (StackId, StudyDate, Score) VALUES (@StackId, @StudyDate, @Score)", new {StackId = session.StackId, StudyDate = session.StudyDate, Score = session.Score});
        }
    }
}
