using Dapper;
using Flashcards.DTOs;
using Flashcards.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards
{
    internal class StudySessionController
    {
        DatabaseManager dbManager = new();
        internal List<StudySessionDTO> GetStudySessions()
        {
            using (var connection = new SqlConnection(dbManager.connectionStringWithDB))
            {
                string cmd = $"SELECT * FROM StudySessions";

                List<StudySessionDTO> result = new List<StudySessionDTO>();
                var studySessions = connection.Query<StudySession>(cmd);

                int dtoID = 1;
                foreach (var studySession in studySessions)
                {
                    result.Add(DTOMapper.toStudySessionDTO(studySession, dtoID));

                    dtoID++;
                }

                return result;

            }
        }

        internal (bool, string?) AddSession(int dtoID, string stackName, double percentage)
        {
            using (var connection = new SqlConnection(dbManager.connectionStringWithDB))
            {
                int originalID = DTOMapper.StackIDMap[dtoID];
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
