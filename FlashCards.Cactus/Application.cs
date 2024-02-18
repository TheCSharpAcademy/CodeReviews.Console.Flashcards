using FlashCards.Cactus.Dao;
using FlashCards.Cactus.Helper;
using FlashCards.Cactus.Service;

namespace FlashCards.Cactus
{
    public class Application
    {
        #region Constants

        /// <summary>
        /// Main menu constants variables.
        /// </summary>
        private const string EXIT_APP = "0";
        private const string MANAGE_STACKS = "1";
        private const string MANAGE_FLASHCARDS = "2";
        private const string STUDY = "3";
        private const string STUDY_REPORT = "4";

        /// <summary>
        /// Stack menu constants variables.
        /// </summary>
        private const string BACK_TO_MAIN = "0";
        private const string SHOW_STACKS = "1";
        private const string ADD_STACK = "2";
        private const string DELETE_STACK = "3";

        /// <summary>
        /// FlashCard menu constants variables.
        /// </summary>
        private const string SHOW_FLASHCARDS = "1";
        private const string ADD_FLASHCARD = "2";
        private const string DELETE_FLASHCARD = "3";
        private const string MODIFY_FLASHCARD = "4";

        /// <summary>
        /// Study menu constants variables.
        /// </summary>
        private const string SHOW_STUDYS = "1";
        private const string START_NEW_STUDY = "2";
        private const string DELETE_STUDY = "3";

        /// <summary>
        /// StudyReport menu constants variables.
        /// </summary>
        private const string SHOW_STUDY_REPORT = "1";

        #endregion Constants

        #region Constructor

        public Application()
        {
            DBHelper.DropTables();
            DBHelper.CreateTables();
            DBHelper.InsertSomeData();

            string connStr = DBHelper.DBConnectionStr;
            StackDao = new StackDao(connStr);
            FlashCardDao = new FlashCardDao(connStr);

            StackService = new StackService(StackDao);
            FlashCardService = new FlashCardService(FlashCardDao);

            StudySessionService = new StudySessionService();
            StudyReportService = new StudyReportService();
        }

        #endregion Constructor

        #region Properties

        public StackDao StackDao { get; set; }

        public FlashCardDao FlashCardDao { get; set; }

        public StackService StackService { get; set; }

        public FlashCardService FlashCardService { get; set; }

        public StudySessionService StudySessionService { get; set; }

        public StudyReportService StudyReportService { get; set; }

        #endregion Properties

        #region Menu
        public void run()
        {
            while (true)
            {
                MenuHelper.PrintMainMenu();
                string? op = Console.ReadLine();
                switch (op)
                {
                    case EXIT_APP:
                        Environment.Exit(0);
                        break;
                    case MANAGE_STACKS:
                        RunStacksManagement();
                        break;
                    case MANAGE_FLASHCARDS:
                        RunFlashCardsManagement();
                        break;
                    case STUDY:
                        RunStudyManagement();
                        break;
                    case STUDY_REPORT:
                        RunStudyReportManagement();
                        break;
                    default:
                        break;
                }
            }
        }

        public void RunStacksManagement()
        {
            while (true)
            {
                MenuHelper.PrintStackManagementMenu();

                string? op = Console.ReadLine();
                Console.Clear();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_STACKS:
                        StackService.ShowStacks();
                        break;
                    case ADD_STACK:
                        StackService.AddStack();
                        break;
                    case DELETE_STACK:
                        StackService.DeleteStack();
                        break;
                    default:
                        break;
                }

                Console.WriteLine("\nPress any key to continue.");
                Console.ReadLine();
            }
        }

        public void RunFlashCardsManagement()
        {
            while (true)
            {
                MenuHelper.PrintFlashCardsManagementMenu();

                string? op = Console.ReadLine();
                Console.Clear();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_FLASHCARDS:
                        FlashCardService.ShowAllFlashCards();
                        break;
                    case ADD_FLASHCARD:
                        FlashCardService.AddFlashCard(StackDao.FindAllStacks());
                        break;
                    case DELETE_FLASHCARD:
                        FlashCardService.DeleteFlashCard();
                        break;
                    case MODIFY_FLASHCARD:
                        FlashCardService.UpdateFlashCard();
                        break;
                    default:
                        break;
                }

                Console.WriteLine("\nPress any key to continue.");
                Console.ReadLine();
            }
        }

        public void RunStudyManagement()
        {
            while (true)
            {
                MenuHelper.PrintStudyManagementMenu();

                string? op = Console.ReadLine();
                Console.Clear();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_STUDYS:
                        StudySessionService.ShowAllStudySessions();
                        break;
                    case START_NEW_STUDY:
                        StudySessionService.StartNewStudySession();
                        break;
                    case DELETE_STUDY:
                        StudySessionService.DeleteStudySession();
                        break;
                    default:
                        break;
                }

                Console.WriteLine("\nPress any key to continue.");
                Console.ReadLine();
            }
        }

        public void RunStudyReportManagement()
        {
            while (true)
            {
                MenuHelper.PrintStudyReportManagementMenu();

                string? op = Console.ReadLine();
                Console.Clear();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_STUDY_REPORT:
                        StudyReportService.ShowStudyReportInSpecificYear();
                        break;
                    default:
                        break;
                }

                Console.WriteLine("\nPress any key to continue.");
                Console.ReadLine();
            }
        }

        #endregion Menu
    }
}
