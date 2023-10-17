using DataAccessLibrary.Models;

namespace DataAccessLibrary.Logic
{
    public class StudySessionsLogic
    {       
        public List<StudySessionsModel> RunStudySession(List<StudySessionsModel> session, List<FlashCardsModel> cards)
        {
            Console.WriteLine($"You are studying stack {session[0].StackName}\n");
            foreach(FlashCardsModel card in cards)
            {
                Console.WriteLine($"{card.Question}\n");
                string userAnswer = Console.ReadLine();
                if( userAnswer.ToLower() == card.Answer.ToLower()) 
                {
                    Console.WriteLine("Correct answer.\n");
                    session[0].TotalAnswerCorrect = session[0].TotalAnswerCorrect+1;
                }
                else
                {
                    Console.WriteLine("Incorrect answer.\n");
                }
            }
            return session;
        }
    }
}
