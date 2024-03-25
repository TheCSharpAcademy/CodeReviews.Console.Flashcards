using Flashcards.JaegerByte.DatabaseHandler;
using Flashcards.JaegerByte.DataModels;
using Spectre.Console;

namespace Flashcards.JaegerByte.Manager
{
    internal class ViewAllTrainingsManager
    {
        public DatabaseTrainingHandler dbhandler { get; set; }
        public void Init(ViewAllTrainingsMenuOption option)
        {
            dbhandler = new DatabaseTrainingHandler();
            while (true)
            {
                switch (option)
                {
                    case ViewAllTrainingsMenuOption.ViewAll:
                        ViewAll();
                        break;
                    case ViewAllTrainingsMenuOption.Exit:
                        break;
                }
                break;
            }
        }

        private void ViewAll()
        {
            List<TrainingSession> trainingSessions = dbhandler.GetAll();

            Grid grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddRow(new string[] {"SessionID", "StackID", "StartDate", "EndDate", "Duration", "CorrectAnswers", "WrongAnswers", "Score" });
            int id = 1;
            foreach (var item in trainingSessions)
            {
                grid.AddRow(new string[] { $"{id}", item.StackID.ToString(), item.StartDate.ToString("g"), item.EndDate.ToString("g"),
                item.Duration.ToString("hh\\:mm"), item.CorrectAnswers.ToString(), item.WrongAnswers.ToString(), item.Score.ToString("P0")});
                id++;
            }
            AnsiConsole.Write(grid);
            AnsiConsole.Write("press ANY key to return");
            Console.ReadKey(true);
        }
    }
}
