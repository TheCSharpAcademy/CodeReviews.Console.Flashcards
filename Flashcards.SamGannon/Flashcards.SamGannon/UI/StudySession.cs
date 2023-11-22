using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.SamGannon.UI
{
    internal class StudySession
    {
        public static void StartStudySession()
        {
            Console.WriteLine("=== Start Study Session ===");
            // Add logic for starting a study session

            // After the study session, return to the main menu
            MainMenu.ShowMenu();
        }
    }
}
