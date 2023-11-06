using CodingTrackerConsoleUI;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using DataAccessLibrary.Logic;

namespace FlashCardsUI
{
    public class RequestStudySessionData
    {
        public static void ReadAllStudySessions(SqlStudySessionsCrud sql)
        {
            var rows = sql.GetAllStudySessions();
            TableVisualisation.ShowTable(rows);
        }
        public static void CreateNewStudySession(SqlStudySessionsCrud sql)
        {
            int stackName = GetNumberInput("Please enter a stack name to study use numbers 1 to 9 only:");
            stackName = CheckValidRecord(sql, stackName);
            DateTime studySessionDate = DateTime.Now;
            int totalAnswerCorrect = 0;
            StudySessionsModel session = new StudySessionsModel
            {
                StackName = stackName,
                StudySessionDate = studySessionDate,
                TotalAnswerCorrect = totalAnswerCorrect
            };             
            List<StudySessionsModel> studySession = sql.CreateStudySession(session);
            List<FlashCardsModel> cards = sql.GetAllFlashCardsByStackName(studySession[0].StackName);
            StudySessionsLogic currentSession = new StudySessionsLogic();
            studySession = currentSession.RunStudySession(studySession, cards);
            Console.WriteLine($"You have answered {studySession[0].TotalAnswerCorrect} questions out of 9 correctly.");
        }
        static int CheckValidRecord(SqlStudySessionsCrud sql, int recordId)
        {
            bool recordExists = sql.CheckRecordExists(recordId);
            while (!recordExists)
            {
                recordId = GetNumberInput("\nStack name does not exist, please enter again. Type 0 to return to Main Menu");
                recordExists = sql.CheckRecordExists(recordId);
            }
            return recordId;
        }
        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string stackNameUserInput = Console.ReadLine();
            if (stackNameUserInput == "0")
            {
                Console.WriteLine("Incorrect choice returning you to Main Menu");
                UserInterface.GetMainMenu();
            }
            int stackName = Validation.CheckValidNumber(stackNameUserInput);
            return stackName;
        }
    }
}
