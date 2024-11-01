using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Ibrahim.Database_Access;
using FlashCards.Ibrahim.Database_Acess;
using FlashCards.Ibrahim.Models;

namespace FlashCards.Ibrahim.UI
{
    public class StudyGame
    {
        public void ShowMenu()
        {
            Console.WriteLine("Your Stacks");
            TableVisualization.ShowTable(Stacks_DB_Access.GetAllStacks());
            Console.WriteLine("\nType in the Stack you wish to study");
            int stack_Id = Stacks_DB_Access.GetOneStack(Console.ReadLine().Trim()).ID;
            Console.Clear();
            List<FlashcardDTO> studyList= Flashcard_DB_Access.GetAllFlashcards(stack_Id);
            int score = 0;

            foreach(FlashcardDTO flashcard in studyList)
            {
                Console.Clear();
                List<string> list = new List<string>();
                list.Add("Front");
                list.Add(flashcard.Front);
                TableVisualization.ShowCard(list);
                Console.Write("\nYour answer: ");
                string answer = Console.ReadLine();

                switch(answer.ToUpper().Trim() == flashcard.Back.ToUpper().Trim())
                {
                    case true:
                        Console.WriteLine("\nNice you got it right!");
                        score++;
                        
                        break;
                    case false:
                        Console.WriteLine($"\nthat's incorrect, the correct answer was {flashcard.Back}");
                        break;
                }
                Console.WriteLine("\nPress any key to go to next card");
                Console.ReadLine();                
            }
            Console.WriteLine($"\nYour score: {score}/{studyList.Count}");
            DateTime date = DateTime.UtcNow;
            StudySession_DB_Access.InsertStudySession(stack_Id, date, score);
            Console.WriteLine("\nPress any key to go to back to main menu");
            Console.ReadLine();
        }
    }
}
