using Flashcards.Data;
using Flashcards.Models;
using Flashcards.Repository;
using Microsoft.Identity.Client;
using System;
using System.Runtime.CompilerServices;

namespace Flashcards.Services
{
    public class UI
    {
        private readonly IStackRepository _stackRepo;
        private readonly IFlashcardRepository _flashcardRepo;
        private readonly IStudySessionRepository _studySessionRepo;

        public UI(IStackRepository stackRepository, IFlashcardRepository flashcardRepository, IStudySessionRepository studySessionRepository)
        {
            _stackRepo = stackRepository;
            _flashcardRepo = flashcardRepository;
            _studySessionRepo = studySessionRepository;
        }

        public void Start()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("==== FlashCards Menu ====");
                Console.WriteLine("1. Create Stack");
                Console.WriteLine("2. Delete Stack");
                Console.WriteLine("3. View Stacks");
                Console.WriteLine("4. Add FlashCard");
                Console.WriteLine("5. View FlashCards in a Stack");
                Console.WriteLine("6. Delete FlashCard");
                Console.WriteLine("7. Study Stack");
                Console.WriteLine("8. View Study Session History");
                Console.WriteLine("0. Exit");

                Console.Write("Choose an option : ");

                var choice = Console.ReadLine();

                switch(choice)
                {
                    case "1": CreateStack(); break;
                    case "2": DeleteStack(); break;
                    case "3": ViewStacks(); break;
                    case "4": AddFlashcard(); break;
                    case "5": ViewFlashcards(); break;
                    case "6": DeleteFlashcard(); break;
                    case "7": StartStudySession(); break;
                    case "8": ViewStudyHistory(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option."); break;
                }

                Console.WriteLine("\n Press any Key to Continue");
                Console.ReadLine();
            }   
        }

        public void CreateStack()
        {
            Console.Clear();
            Console.Write("Please Enter Stack Name : ");
            var stackName = Console.ReadLine();
            _stackRepo.AddStack(stackName);
            Console.WriteLine("Stack created.");
        }
        public void DeleteStack()
        {
            ViewStacks();
            Console.Write("Enter Stack Id to delete: ");
            if(int.TryParse(Console.ReadLine(), out int id))
            {
                int actualId = MapDisplayIdToActualId("stacks", id);
                _stackRepo.DeleteStack(actualId);
                Console.WriteLine("Stack deleted.");
                ViewStacks();
            }
        }
        public void ViewStacks()
        {
            var stacks = _stackRepo.GetAllStacks();
            Console.WriteLine("\n==== Stacks ====");

            for(int i = 0; i < stacks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {stacks[i].Name}");
            }
        }
        public void AddFlashcard()
        {
            ViewStacks();
            Console.Write("Enter the Stack Id Where You want to add a flashcard : ");
            if (int.TryParse(Console.ReadLine(), out int Id))
            {
                int actualId = MapDisplayIdToActualId("stacks", Id);
                Console.Write("Enter question: ");
                var q = Console.ReadLine();
                Console.Write("Enter answer: ");
                var a = Console.ReadLine();
                _flashcardRepo.AddFlashcard(actualId, q, a);
                Console.WriteLine("Flashcard Added.");
            }
        }

        public void ViewFlashcards()
        {
            ViewStacks();
            Console.Write("Enter Stack Id to View: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                int actualId = MapDisplayIdToActualId("stacks", id);
                var cards = _flashcardRepo.GetFlashcardsByStack(actualId);

                for (int i = 0; i < cards.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Q) {cards[i].Question} A) {cards[i].Answer}");
                }
            }
        }
        public void DeleteFlashcard()
        {
            ViewStacks();
            Console.Write("Enter Stack Id: ");
            if (int.TryParse(Console.ReadLine(), out int stackId))
            {
                int actualStackId = MapDisplayIdToActualId("stacks", stackId);
                var cards = _flashcardRepo.GetFlashcardsByStack(actualStackId);

                for (int i = 0; i < cards.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Q) {cards[i].Question} A) {cards[i].Answer}");
                }

                Console.Write("Enter FlashCard Id you want to delete : ");
                if (int.TryParse(Console.ReadLine(), out int Id))
                {
                    int actualId = MapDisplayIdToActualId("flashcards", Id, actualStackId);
                    _flashcardRepo.DeleteFlashcard(actualId);
                }
            }
        }
        public void StartStudySession()
        {
            int score = 0;
            DateTime dateTime = DateTime.Now;

            ViewStacks();
            Console.Write("Enter Stack Id: ");
            if (int.TryParse(Console.ReadLine(), out int stackId))
            {
                int actualStackId = MapDisplayIdToActualId("stacks", stackId);
                var cards = _flashcardRepo.GetFlashcardsByStack(actualStackId);

                for (int i = 0; i < cards.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Q) {cards[i].Question}");
                    Console.Write("Enter the answer : ");
                    string ans = Console.ReadLine();
                    if (ans == cards[i].Answer)
                    {
                        score++;
                        Console.WriteLine("It's the correct answer!");
                    }
                    else
                    {
                        Console.WriteLine($"It's not the correct answer! The correct answer is {cards[i].Answer}");
                    }
                }

                StudySession session = new StudySession()
                {
                    StackId = actualStackId,
                    Score = score,
                    StudyDate = dateTime,
                };

                _studySessionRepo.SaveStudySession(session);
            }
        }
        public void ViewStudyHistory()
        {
            var li = _studySessionRepo.GetAllSessions();
            Console.WriteLine("===== SESSION DETAILS =====");
            for (int i = 0; i < li.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {li[i].StackName} {li[i].StudyDate} {li[i].Score}");
            }
        }

        public int MapDisplayIdToActualId(string tableName, int displayId, int stackId = 0)
        {
            if(tableName == "stacks")
            {
                var stackList = _stackRepo.GetAllStacks();
                return stackList[displayId - 1].Id;
            }
            else if(tableName == "flashcards")
            {
                var flashcardList = _flashcardRepo.GetFlashcardsByStack(stackId);
                return flashcardList[displayId - 1].Id;
            }
            return 0;
        }
    }
}
