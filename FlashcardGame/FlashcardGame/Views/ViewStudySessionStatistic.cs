using System.Globalization;
using FlashcardGame.Models;

namespace FlashcardGame.Views
{
    internal class ViewStudySessionStatistic
    {
        public static void ViewStudySessionStats()
        {
            Console.Clear();
            List<Stack> stacks = DataAccess.GetStacks();
            List<StudySession> sessions = DataAccess.GetStudySessions();
            Stack chosenStack = new Stack();
            Console.WriteLine("What stack statistics you want to see, enter full name or press 0 to exit?");
            for (int i = 0; i < stacks.Count; i++)
            {
                Stack stack = stacks[i];
                Console.WriteLine($"{i + 1}. {stack.stack_name}");
            }
            string? input = Console.ReadLine();
            foreach (var stack in stacks)
            {
                if (stack.stack_name == input)
                {
                    chosenStack = stack;
                }
            }
            while (!stacks.Any(s => s.stack_name == input))
            {
                if (input == "0")
                {
                    return;
                }
                Console.WriteLine("Wrong name try again!");
                input = Console.ReadLine();
            }

            Console.WriteLine("Write what year statistics of your study data you want to see or press 0 to exit");
            input = Console.ReadLine();
            if (input == "0")
            {
                return;
            }

            int parsedYear;
            while (!int.TryParse(input, out parsedYear))
            {
                Console.WriteLine("Invalid year, please try again");
                input = Console.ReadLine();
            }

            var dateTimes = sessions
                .Where(s => s.Stack_Id == chosenStack.stack_id)
                .Select(g => g.Study_Date)
                .ToList();

            var monthsBySessionss = dateTimes
                .Where(d => d.Year == parsedYear)
                .GroupBy(s => s.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                });

            var allMonths = Enumerable.Range(1, 12).Select(m => new MonthData(m)).ToList();

            foreach (var months in allMonths)
            {
                foreach (var list in monthsBySessionss)
                {
                    if (months.Month == list.Month)
                    {
                        months.Count = list.Count;
                    }
                }
            }

            Console.WriteLine($"\nYear:{parsedYear}  Stack:{chosenStack.stack_name}");
            Console.WriteLine(new string('-', 50));
            foreach (var month in allMonths)
            {
                string monthName = new DateTime(1, month.Month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("en-US"));
                Console.WriteLine($"{monthName}" + " - " + $"{month.Count}");
            }
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();

        }

        public class MonthData
        {
            public int Month { get; set; }
            public int Count { get; set; }

            public MonthData(int month)
            {
                Month = month;
                Count = 0;
            }
        }
    }
}
