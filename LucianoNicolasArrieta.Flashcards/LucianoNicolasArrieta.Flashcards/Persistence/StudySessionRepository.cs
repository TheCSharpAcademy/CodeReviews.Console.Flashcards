using ConsoleTableExt;
using LucianoNicolasArrieta.Flashcards.DTOs;
using LucianoNicolasArrieta.Flashcards.Model;
using System.Data.SqlClient;
using Stack = LucianoNicolasArrieta.Flashcards.Model.Stack;

namespace LucianoNicolasArrieta.Flashcards.Persistence
{
    public class StudySessionRepository
    {
        private string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");

        public void Insert(StudySession studySession)
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO StudySession (StackId, SessionDate, Questions, CorrectAnswers) VALUES (@stackId, @sessionDate, @questions, @correctAnswers)";
                SqlCommand command = new SqlCommand(query, myConnection);

                myConnection.Open();

                command.Parameters.AddWithValue("@stackId", studySession.Stack.Id);
                command.Parameters.AddWithValue("@sessionDate", studySession.Date);
                command.Parameters.AddWithValue("@questions", studySession.TotalQuestions);
                command.Parameters.AddWithValue("@correctAnswers", studySession.CorrectAnswers);
                command.ExecuteNonQuery();
            }

            Console.Clear();
            Console.WriteLine($"Study Session saved successfully!");
        }

        public void PrintAll()
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                string query = $"SELECT * FROM StudySession";
                SqlCommand command = new SqlCommand(query, myConnection);

                myConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                List<StudySessionDTO> flashcards = new List<StudySessionDTO>();
                StackRepository stackRepository = new StackRepository();
                while (reader.Read())
                {
                    StudySessionDTO aux = new StudySessionDTO();

                    Stack auxStack = stackRepository.GetStack(reader.GetInt32(1));
                    aux.Subject = auxStack.Subject;

                    aux.Date = reader.GetDateTime(2).ToString("dd/MM/yyyy");
                    aux.TotalQuestions = reader.GetInt32(3);
                    aux.CorrectAnswers = reader.GetInt32(4);

                    flashcards.Add(aux);
                }

                ConsoleTableBuilder.From(flashcards)
                    .WithCharMapDefinition(
                        CharMapDefinition.FramePipDefinition,
                        new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                        })
                    .WithTitle("STUDY SESSIONS")
                    .ExportAndWriteLine(TableAligntment.Left);
            }
        }
    }
}
