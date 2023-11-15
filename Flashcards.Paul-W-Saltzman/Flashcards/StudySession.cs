
namespace Flashcards
{
    internal class StudySession
    {
        internal int StudySessionID { get; set; }
        internal int StackID { get; set; }
        internal DateOnly Date { get; set; } 
        internal string StackName { get; set; }
        internal int Correct { get; set; }
        internal int Total { get; set; }
        internal double Score { get; set; }

        internal StudySession()
        {

        }

        private StudySession(int stackID, DateOnly date, string stackName, int correct, int total )
        { 
            this.StackID = stackID;
            this.Date = date;
            this.StackName = stackName;
            this.Correct = correct;
            this.Total = total;
            this.Score = correct/total;
            this.StudySessionID = Data.EnterStudySession(this);
        }

        internal static void LoadSeedDataStudySessions()
        {
            StudySession studysession = new StudySession(1, new DateOnly(2021, 11, 1), "Variable Types", 4, 2);
            StudySession studysession1 = new StudySession(1, new DateOnly(2022, 10, 20), "Variable Types", 4, 2); 
            StudySession studysession3 = new StudySession(1, new DateOnly(2023, 9, 21), "Variable Types", 4, 2); 
            StudySession studysession4 = new StudySession(2, new DateOnly(2021, 8, 2), "Selector Codes", 4, 2); 
            StudySession studysession5 = new StudySession(2, new DateOnly(2022, 7, 3), "Selector Codes", 4, 2); 
            StudySession studysession6 = new StudySession(2, new DateOnly(2023, 6, 4), "Selector Codes", 4, 2); 
            StudySession studysession7 = new StudySession(3, new DateOnly(2020, 5, 10), "French", 4, 2); 
            StudySession studysession8 = new StudySession(3, new DateOnly(2022, 4, 12), "French", 4, 2); 
            StudySession studysession9 = new StudySession(3, new DateOnly(2023, 3, 13), "French", 4, 2); 
            StudySession studysession10 = new StudySession(4, new DateOnly(2020, 2, 15), "Vietnamese", 4, 2); 
            StudySession studysession11 = new StudySession(4, new DateOnly(2021, 1, 20), "Vietnamese", 4, 2); 
            StudySession studysession12 = new StudySession(4, new DateOnly(2022, 12, 21), "Vietnamese", 4, 2); 
            StudySession studysession13 = new StudySession(5, new DateOnly(2020, 11, 22), "Spanish", 4, 2); 
            StudySession studysession14 = new StudySession(5, new DateOnly(2021, 10, 25), "Spanish", 4, 2);
            StudySession studysession15 = new StudySession(5, new DateOnly(2023, 9, 30), "Spanish", 4, 2);
        }

        public static void Study(List<DTO_StackAndCard> study)
        {
            DTO_StackAndCard stack = study[0];
            int stackID = stack.StackID;
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            string stackName = stack.StackName;
            int correct = 0;
            int total = study.Count;

            
            foreach (DTO_StackAndCard card in study)
            {
                Console.Clear();
                Helpers.ViewCard(card, true);
                Console.WriteLine();
                Console.Write("Awnser: ");
                Console.CursorVisible = true;
                string input = Console.ReadLine();
                bool match = Helpers.CompareStrings(input, card.CardBack);
                Console.CursorVisible = false;
               if (match == true)
               {
                    Console.WriteLine("Correct");
                    Console.ReadKey();
                    correct++;
                }
               else if(match == false)
               {
                    Console.WriteLine("Incorrect");
                    Console.WriteLine($@"You entered: {input}");
                    Console.WriteLine("The Awnser is:");
                    Helpers.ViewCard(card,false);
                    Console.ReadKey();
                }


            }
            StudySession thisStudySession = new StudySession(stackID,date,stackName,correct,total);
            Console.WriteLine($@"You got {correct} out of {total} correct.");
            Console.ReadLine(); 
        }
    }
}
