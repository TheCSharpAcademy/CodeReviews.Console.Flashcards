using Azure;
using Spectre.Console;
using System.Diagnostics;

namespace FlashCards
{
    internal class StudySessionRepositoryService
    {
        public StudySessionRepository StudySessionRepository { get; set; }

        public StudySessionRepositoryService(StudySessionRepository repository)
        {
            StudySessionRepository = repository;
        }
        public void PrepareRepository(List<CardStack> stacks, List<StudySession> defaultData)
        {

            if (!StudySessionRepository.DoesTableExist())
            {
                StudySessionRepository.CreateTable();
                StudySessionRepository.AutoFill(stacks, defaultData);
            }
        }

        public void NewStudySession(CardStack stack, List<FlashCardDto> cards)
        {
            int score = 0;
            Random random = new Random();
            string input = string.Empty;

            while(input != "exit")
            {
                FlashCardDto card = cards[random.Next(cards.Count)];

                PrintCard(stack.StackName, card.FrontText);

                input = GetAnswer();

                if (input == card.BackText.ToLower())
                {
                    Console.WriteLine("Correct!");
                    score++;
                }
                else
                {
                    Console.WriteLine("Incorrect!");
                    Console.WriteLine($"Correct answer is {card.BackText}");
                }
                Console.WriteLine("####################################");
            }

            InsertSession(new StudySession()
            {
                StackName = stack.StackName,
                StackId = stack.StackID,
                SessionDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month, DateTime.Now.Day),
                Score = score,
            });

        }
        private void InsertSession(StudySession session) => StudySessionRepository.Insert(session);
        private void PrintCard(string stack, string frontText)
        {
            var table = new Table();
            table.AddColumn("Stack");
            table.AddColumn("Front Side");
            table.AddRow(stack, frontText);

            AnsiConsole.Write(table);
        }
        private string GetAnswer()
        {
            Console.WriteLine("Enter your answer: ");
            string? input = Console.ReadLine();

            return input == null ? string.Empty : input.ToLower();
        }
        public int ValidateAnswer(FlashCardDto card, string answer)
        {
            if(answer == "exit")
            {
                return -1;
            }
            if (answer == card.BackText)
            {
                Console.WriteLine("Correct!");
                return 1;
            }
            else 
            {
                Console.WriteLine("Incorrect!");
                return 0;
            }
        }
    }
}
