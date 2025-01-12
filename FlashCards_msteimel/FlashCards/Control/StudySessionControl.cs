using FlashCards.Database;
using FlashCards.Models;

namespace FlashCards.Control
{
    internal static class StudySessionControl
    {
        internal static void StartStudySession()
        {
            int correctAnswers = 0;
            int question = 1;
            string answer = null;
            int stackId = GetStackID(out string stackName);

            if (stackId == -1)
            {
                return;
            }

            List<FlashCardDto> flashCards = FlashCardDBOperations.GetFlashCardDtoByStackId(stackId);
            int numQuestions = flashCards.Count;

            if (numQuestions < 1)
            {
                Console.WriteLine("This stack is empty. Press any key to return");
                Console.ReadLine();
                return;
            }

            foreach (FlashCardDto flashCard in flashCards)
            {
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("STUDY SESSION IN PROCESS");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine($"Question {question} of {flashCards.Count}\n");
                Console.WriteLine($"{flashCard.Question}\n");

                Console.WriteLine("Enter answer:\n");
                answer = Helper.GetStringInput();

                if (answer == flashCard.Answer)
                {
                    correctAnswers++;
                    Console.WriteLine("Correct\n");
                }
                else
                {
                    Console.WriteLine("Incorrect");
                    Console.WriteLine($"The correct answer is {flashCard.Answer}\n");
                }

                Console.WriteLine("Enter any key for next question or enter 0 to return to menu");
                answer = Console.ReadLine();

                if (answer == "0")
                {
                    Console.Clear();
                    return;
                }
                Console.Clear();
                question++;
            }

            StudySession studySession = new StudySession
            {
                Date = DateTime.Now.ToString("MM/dd/yy"),
                Score = (correctAnswers / (double)numQuestions).ToString("P0"),
                StackId = stackId,
                StackName = stackName
            };

            Console.WriteLine($"You got {correctAnswers} out of {numQuestions} correct, for a score of {studySession.Score}\n");

            Console.WriteLine("Press enter to continue");
            Console.ReadLine();

            Console.Clear();

            StudyDBOperations.AddSession(studySession);
        }

        internal static int GetStackID(out string stackName)
        {
            Console.Clear();
            List<Stack> stacks = StackDBOperations.GetStacks();
            List<StackWithCleanId> cleanStacks = StackControl.LaunderStackId(stacks);
            StackControl.ViewStacks(cleanStacks);

            while (true)
            {
                Console.WriteLine("Enter the ID of the flashcard stack, or enter 0 to return to menu.");

                string stackID = Helper.GetIDInput();

                if (stackID == "0")
                {
                    Console.Clear();
                    stackName = null;
                    return -1;
                }

                int id = int.Parse(stackID);

                foreach (StackWithCleanId stack in cleanStacks)
                {
                    if (stack.CleanId == id)
                    {
                        Console.Clear();
                        stackName = stack.Name;
                        return stack.Id;
                    }
                }

                Console.WriteLine("Stack not found. Please try again.");
            }
        }

        internal static void ViewStudySessions()
        {
            List<StudySession> sessions = StudyDBOperations.GetStudySessions();

            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("STUDY SESSIONS:");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("********************************************************************************");
            foreach (StudySession session in sessions)
            {
                Console.WriteLine($"Date: {session.Date} --- Score: {session.Score} --- Stack Name: {session.StackName}");
            }
            Console.WriteLine("********************************************************************************\n");

            Console.WriteLine("Press any key to return");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
