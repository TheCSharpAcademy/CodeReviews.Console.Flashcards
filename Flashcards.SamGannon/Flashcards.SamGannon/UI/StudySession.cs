using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.SamGannon.UI
{
    internal class StudySession
    {
        private Timer timer;
        private int studySessionDurationSeconds;
        private DateTime startTime;

        public static void StartStudySession()
        {
            Console.WriteLine("=== Start Study Session ===");
            // Add logic for starting a study session
            // Show list of stacks

            Console.WriteLine("Please select the ID of the flashcard stack you wish to study");
            var selectedStackId = Console.ReadLine();

            while (!int.TryParse(selectedStackId, out _))
            {
                Console.WriteLine("You must select an Id number of a listed stack");
                selectedStackId = Console.ReadLine();
            }

            // Call the database and get flashcards associated with the stack.

            var studySession = new StudySession();
            studySession.StartTimer();

            // Wait for the user to finish the study session
            Console.WriteLine("Press any key to end the study session.");
            Console.ReadKey();

            // Stop the timer and record the study session duration
            studySession.StopTimer();

            // Insert study session details into the database
            studySession.InsertStudySessionIntoDatabase();

            // After the study session, return to the main menu
            MainMenu.ShowMenu();
        }

        private void StartTimer()
        {
            startTime = DateTime.Now;
            timer = new Timer(TimerCallback, null, 0, 1000);
        }

        private void TimerCallback(object state)
        {
            studySessionDurationSeconds++;
            // Console.WriteLine($"Study session duration: {studySessionDurationSeconds} seconds");
        }

        private void StopTimer()
        {
            timer.Dispose();
        }

        private void InsertStudySessionIntoDatabase()
        {
            // TODO: Insert study session details into the database
            // Use the startTime and studySessionDurationSeconds to record the study session duration
            Console.WriteLine($"Study session duration: {studySessionDurationSeconds} seconds");

            // DatabaseHelper.InsertStudySession(startTime, studySessionDurationSeconds);
        }
    }

}
