using FlashCards.Cactus.DataModel;

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
        private const string START_EXISTING_STUDY = "2";
        private const string START_NEW_STUDY = "3";
        private const string DELETE_STUDY = "4";

        /// <summary>
        /// StudyReport menu constants variables.
        /// </summary>
        private const string SHOW_STUDY_REPORT = "1";

        #endregion Constants

        #region Constructor

        public Application()
        {
            StackMangement = new StackMangement();
            FlashCardManagement = new FlashCardManagement();
            StudySessionManagement = new StudySessionManagement();
        }

        #endregion Constructor

        #region Properties

        public StackMangement StackMangement { get; set; }

        public FlashCardManagement FlashCardManagement { get; set; }

        public StudySessionManagement StudySessionManagement { get; set; }

        #endregion Properties

        #region Menu
        public void run()
        {
            while (true)
            {
                MenuUtils.PrintMainMenu();
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
                MenuUtils.PrintStackManagementMenu();

                string? op = Console.ReadLine();
                Console.Clear();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_STACKS:
                        StackMangement.ShowStacks();
                        break;
                    case ADD_STACK:
                        StackMangement.AddStack();
                        break;
                    case DELETE_STACK:
                        StackMangement.DeleteStack();
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
                MenuUtils.PrintFlashCardsManagementMenu();

                string? op = Console.ReadLine();
                Console.Clear();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_FLASHCARDS:
                        FlashCardManagement.ShowAllFlashCards();
                        break;
                    case ADD_FLASHCARD:
                        FlashCardManagement.AddFlashCard();
                        break;
                    case DELETE_FLASHCARD:
                        FlashCardManagement.DeleteFlashCard();
                        break;
                    case MODIFY_FLASHCARD:
                        FlashCardManagement.ModifyFlashCard();
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
                MenuUtils.PrintStudyManagementMenu();

                string? op = Console.ReadLine();
                Console.Clear();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_STUDYS:
                        StudySessionManagement.ShowAllStudySessions();
                        break;
                    case START_EXISTING_STUDY:
                        StudySessionManagement.StartFromExistingStudySession();
                        break;
                    case START_NEW_STUDY:
                        StudySessionManagement.StartNewStudySession();
                        break;
                    case DELETE_STUDY:
                        StudySessionManagement.DeleteStudySession();
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
                MenuUtils.PrintStudyReportManagementMenu();

                string? op = Console.ReadLine();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_STUDY_REPORT:
                        Console.WriteLine("Show  the study report from a specific year.");
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion Menu
    }
}
