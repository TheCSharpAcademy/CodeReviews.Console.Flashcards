using Flash.Helper.DTO;
using Flash.Helper.Renumber;
using System.Data.SqlClient;

namespace Flash.Helper.MainHelper;
internal class ShowStudyHistory
{
    internal static void GetShowStudyHistory()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");

                string selectQuery = $@"
                            SELECT Study_Primary_Id , Date, Score, Stack_Primary_Id
                            FROM Study";

                List<StudyDto> studys = new List<StudyDto>();
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                StudyDto study = new StudyDto
                                {
                                    Study_Primary_Id = reader.GetInt32(0),
                                    Date = reader.GetDateTime(1),
                                    Score = reader.GetString(2),
                                    Stack_Primary_Id = reader.GetInt32(3)
                                };
                                studys.Add(study);
                            }

                            RenumberStudy.GetRenumberStudys(studys);

                            foreach (var study in studys)
                            {
                                Console.WriteLine(@$"
                                    Study_Primary_Id: {study.Study_Primary_Id}, 
                                    Date: {study.Date}, 
                                    Score: {study.Score}, 
                                    Stack_Primary_Id: {study.Stack_Primary_Id}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No flashcards found.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
