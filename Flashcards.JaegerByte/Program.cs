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
        public static MainMenuManager mainMenuManager { get; set; }
        public static FlashcardsManager flashcardsManager { get; set; }
        public static StacksManager stacksManager { get; set; }
        public static TrainingManager trainingManager { get; set; }
        public static ViewAllTrainingsManager viewAllTrainingsManager { get; set; }
        public static string ConnectionString { get; set; }
        #endregion

        static void Main()
        {
            ApplicationSetup();

            while (true)
            {
                Console.Clear();
                MainMenuOption selection = mainMenuManager.GetMainMenuSelection();
                switch (selection)
                {
                    case MainMenuOption.ManageStacks:
                        stacksManager.Init(mainMenuManager.GetManageStacksMenuSelection());
                        break;
                    case MainMenuOption.ManageFlashcards:
                        flashcardsManager.Init(mainMenuManager.GetManageFlashcardsMenuSelection());
                        break;
                    case MainMenuOption.Training:
                        trainingManager.Init(mainMenuManager.GetTrainingMenuSelection());
                        break;
                    case MainMenuOption.ViewTrainingSessions:
                        viewAllTrainingsManager.Init(mainMenuManager.GetAllTrainingsMenuSelection());
                        break;
                    case MainMenuOption.Exit:
                        System.Environment.Exit(0);
                        break;
                }
            }
        }

        static void ApplicationSetup()
        {
            mainMenuManager = new MainMenuManager();
            flashcardsManager = new FlashcardsManager();
            stacksManager = new StacksManager();
            trainingManager = new TrainingManager();
            viewAllTrainingsManager = new ViewAllTrainingsManager();
            ConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");
        }
    }
}
