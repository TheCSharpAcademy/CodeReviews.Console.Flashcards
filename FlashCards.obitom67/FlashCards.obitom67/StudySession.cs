using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using Spectre.Console;

namespace FlashCards.obitom67
{
    internal class StudySession
    {

        int StudyId { get; set; }
        int StackId { get; set; }
        int CorrectQ {  get; set; }
        int TotalQ { get; set; }
        string Date { get; set; }


        public static void StartSession()
        {
            StudySession studySes = new StudySession();
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            var currentStack = Stack.DisplayStacks();
            string answer;
            using (var connection = new SqlConnection(connectionString))
            {

                string flashcardsSql = $"SELECT * FROM dbo.Flashcard WHERE StackId = {currentStack.StackId}";
                var flashcardSelect = connection.Query(flashcardsSql);
                string studySesSql = $"SELECT COUNT(*) FROM dbo.StudySessions";
                var studySesCount = connection.ExecuteScalar(studySesSql);
                studySes.StudyId = (int)studySesCount + 1;
                studySes.StackId = currentStack.StackId;
                do
                {
                    Random random = new Random();
                    int randInd = random.Next(flashcardSelect.Count());
                    var randFC = flashcardSelect.ElementAt(randInd);
                    AnsiConsole.WriteLine($"{randFC.FrontText}");
                    answer = AnsiConsole.Ask<string>("Please enter the answer or type 0 to exit.");
                    if(answer == randFC.BackText)
                    {
                        studySes.CorrectQ += 1;
                        studySes.TotalQ += 1;
                    }
                    else
                    {
                        studySes.TotalQ += 1;
                    }
                }
                while (answer != "0");
                studySes.TotalQ -= 1;
                StudySession.RecordSession(studySes);

            }                       
        }

        public static void RecordSession(StudySession studySes)
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            using(var connection = new SqlConnection(connectionString))
            {
                string insertSes = $"INSERT INTO StudySessions(StudyId, StackId, CorrectQ, TotalQ, Date) VALUES ({studySes.StudyId},{studySes.StackId}, {studySes.CorrectQ},{studySes.TotalQ},'{DateTime.Now.Date.ToString("dd/MM/yyyy")}')";
                connection.Execute(insertSes);
            }
        }


        public static void ShowRecords()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            using(var connection = new SqlConnection(connectionString))
            {
                string stacksRead = $"SELECT * FROM dbo.Stack";
                var stackObj = connection.Query<Stack>(stacksRead);

                string sesRead = $"SELECT * FROM dbo.StudySessions";
                var sesObj = connection.Query<StudySession>(sesRead);
                AnsiConsole.WriteLine(" ");
                AnsiConsole.Write(new Columns(new Text("Stack Name"),new Text("Score"),new Text("Date")));

                foreach(StudySession s in sesObj)
                {
                    Stack studyStack = stackObj.Where(x => x.StackId == s.StackId).First();
                    string score = $"{s.CorrectQ}/{s.TotalQ}";
                    AnsiConsole.Write(new Columns(new Text(studyStack.StackName),new Text(score), new Text(s.Date)));
                }
                AnsiConsole.WriteLine(" ");

            }
        }
    }
}
