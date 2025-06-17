using Dapper;
using Flashcards.DTOs;
using Flashcards.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards
{
    internal class StudySessionController
    {
        DatabaseManager dbManager = new();
        internal List<StudySessionDto> GetStudySessions()
        {
            using (var connection = new SqlConnection(dbManager.connectionStringWithDB))
            {
                string cmd = $"SELECT * FROM StudySessions";

                List<StudySessionDto> result = new List<StudySessionDto>();
                var studySessions = connection.Query<StudySession>(cmd);

                int dtoID = 1;
                foreach (var studySession in studySessions)
                {
                    result.Add(DtoMapper.ToStudySessionDto(studySession, dtoID));

                    dtoID++;
                }

                return result;

            }
        }

        internal (bool, string?) AddSession(int dtoID, string stackName, double percentage)
        {
            using (var connection = new SqlConnection(dbManager.connectionStringWithDB))
            {
                int originalID = DtoMapper.StackIDMap[dtoID];
                DateTime currentDate = DateTime.Now;
                var cmd = $"INSERT INTO StudySessions VALUES ('{currentDate}', '{stackName}',{percentage}, {originalID})";

                try
                {
                    connection.Execute(cmd);
                    return (true, null);
                }

                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }
    }
}
