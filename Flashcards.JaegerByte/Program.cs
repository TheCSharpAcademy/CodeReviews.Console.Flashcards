using Flashcards.JaegerByte.Manager;

namespace Flashcards.JaegerByte
{
    enum MainMenuOption {ManageStacks, ManageFlashcards, Training, ViewTrainingSessions, Exit}
    enum StacksMenuOption {AddStack, DeleteStack, ViewStack, Exit}
    enum FlashcardsMenuOption {AddFlashcard, DeleteFlashcard, ViewAllFlashcards, Exit}
    enum ViewAllTrainingsMenuOption {ViewAll, Exit}
    enum TrainingMenuOption {StartNewSession, Exit}

    internal class Program
    {
        #region Prop
        public static MainMenuManager MainMenuManager { get; set; }
        public static FlashcardsManager FlashcardsManager { get; set; }
        public static StacksManager StacksManager { get; set; }
        public static TrainingManager TrainingManager { get; set; }
        public static ViewAllTrainingsManager ViewAllTrainingsManager { get; set; }
        public static string ConnectionString { get; set; }
        #endregion

        static void Main()
        {
            ApplicationSetup();

            while (true)
            {
                Console.Clear();
                MainMenuOption selection = MainMenuManager.GetMainMenuSelection();
                switch (selection)
                {
                    case MainMenuOption.ManageStacks:
                        StacksManager.Init(MainMenuManager.GetManageStacksMenuSelection());
                        break;
                    case MainMenuOption.ManageFlashcards:
                        FlashcardsManager.Init(MainMenuManager.GetManageFlashcardsMenuSelection());
                        break;
                    case MainMenuOption.Training:
                        TrainingManager.Init(MainMenuManager.GetTrainingMenuSelection());
                        break;
                    case MainMenuOption.ViewTrainingSessions:
                        ViewAllTrainingsManager.Init(MainMenuManager.GetAllTrainingsMenuSelection());
                        break;
                    case MainMenuOption.Exit:
                        System.Environment.Exit(0);
                        break;
                }
            }
        }

        static void ApplicationSetup()
        {
            MainMenuManager = new MainMenuManager();
            FlashcardsManager = new FlashcardsManager();
            StacksManager = new StacksManager();
            TrainingManager = new TrainingManager();
            ViewAllTrainingsManager = new ViewAllTrainingsManager();
            ConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");
        }
    }
}
