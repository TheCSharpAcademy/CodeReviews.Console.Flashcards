using Flashcards.JaegerByte.Manager;
using System.Data.SqlClient;
using Dapper;

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
        public static string DatabaseName { get; set; }
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
            DatabaseName = System.Configuration.ConfigurationManager.AppSettings.Get("DatabaseName");
            CreateTables();
        }
        static void CreateTables()
        {
            using (SqlConnection initialConnection = new SqlConnection(ConnectionString))
            {
                initialConnection.Open();

                int result = initialConnection.QueryFirstOrDefault<int>($"SELECT COUNT(*) FROM sys.databases WHERE name = '{DatabaseName}'");
                if (result == 0)
                {
                    initialConnection.Execute($"CREATE DATABASE {DatabaseName}");
                }
                initialConnection.Close();
            }
            ConnectionString += DatabaseName;
            using SqlConnection connection = new SqlConnection(ConnectionString);
            {
                connection.Open();
                connection.Execute(@"IF OBJECT_ID(N'tblStacks', N'U') IS NULL
                                CREATE TABLE tblStacks
                                (
                                StackID INT PRIMARY KEY IDENTITY,
                                Title TEXT
                                )");
                connection.Execute(@"IF OBJECT_ID(N'tblFlashcards', N'U') IS NULL
                                CREATE TABLE tblFlashcards
                                (
                                FlashcardID INT PRIMARY KEY IDENTITY,
                                Question TEXT,
                                Answer TEXT,
                                StackID INT
                                FOREIGN KEY (StackID) REFERENCES tblStacks(StackID)
                                )");
                connection.Execute(@"IF OBJECT_ID(N'tblTrainingSessions', N'U') IS NULL
                                CREATE TABLE tblTrainingSessions
                                (
                                SessionID INT PRIMARY KEY IDENTITY,
                                StackID INT
                                FOREIGN KEY (StackID) REFERENCES tblStacks(StackID),
                                StartDate TEXT,
                                EndDate TEXT,
                                CorrectAnswers INT,
                                WrongAnswers INT
                                )");
            }
        }
    }
}
