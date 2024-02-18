using FlashCards.Cactus.Dao;
using FlashCards.Cactus.Helper;
using FlashCards.Cactus.Service;

namespace FlashCards.Cactus;

public class Application
{
    #region Constructor

    public Application()
    {
        init();
    }

    #endregion Constructor

    #region Properties

    public StackDao StackDao { get; set; }

    public FlashCardDao FlashCardDao { get; set; }

    public StudySessionDao StudySessionDao { get; set; }

    public StackService StackService { get; set; }

    public FlashCardService FlashCardService { get; set; }

    public StudySessionService StudySessionService { get; set; }

    public StudyReportService StudyReportService { get; set; }

    #endregion Properties

    #region Methods

    public void init()
    {
        DBHelper.DropTables();
        DBHelper.CreateTables();
        DBHelper.InsertSomeData();

        string connStr = DBHelper.DBConnectionStr;
        StackDao = new StackDao(connStr);
        FlashCardDao = new FlashCardDao(connStr);
        StudySessionDao = new StudySessionDao(connStr);

        StackService = new StackService(StackDao);
        FlashCardService = new FlashCardService(FlashCardDao);
        StudySessionService = new StudySessionService(StudySessionDao);
        StudyReportService = new StudyReportService(StudySessionDao);
    }

    public void UpdateCache()
    {
        StackService.UpdateCache();
        FlashCardService.UpdateCache();
        StudySessionService.UpdateCache();
    }

    #endregion Methods

    #region Menu
    public void run()
    {
        while (true)
        {
            MenuHelper.PrintMainMenu();
            string? op = Console.ReadLine();
            switch (op)
            {
                case Constants.EXIT_APP:
                    Environment.Exit(0);
                    break;
                case Constants.MANAGE_STACKS:
                    RunStackService();
                    break;
                case Constants.MANAGE_FLASHCARDS:
                    RunFlashCardService();
                    break;
                case Constants.STUDY:
                    RunStudySessionService();
                    break;
                case Constants.STUDY_REPORT:
                    RunStudyReportService();
                    break;
                default:
                    break;
            }
        }
    }

    public void RunStackService()
    {
        while (true)
        {
            MenuHelper.PrintStackManagementMenu();

            string? op = Console.ReadLine();
            Console.Clear();
            switch (op)
            {
                case Constants.BACK_TO_MAIN:
                    return;
                case Constants.SHOW_STACKS:
                    StackService.ShowStacks();
                    break;
                case Constants.ADD_STACK:
                    StackService.AddStack();
                    UpdateCache();
                    break;
                case Constants.DELETE_STACK:
                    StackService.DeleteStack(FlashCardDao);
                    UpdateCache();
                    break;
                default:
                    break;
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadLine();
        }
    }

    public void RunFlashCardService()
    {
        while (true)
        {
            MenuHelper.PrintFlashCardsManagementMenu();

            string? op = Console.ReadLine();
            Console.Clear();
            switch (op)
            {
                case Constants.BACK_TO_MAIN:
                    return;
                case Constants.SHOW_FLASHCARDS:
                    FlashCardService.ShowAllFlashCards();
                    break;
                case Constants.ADD_FLASHCARD:
                    FlashCardService.AddFlashCard(StackDao.FindAll());
                    UpdateCache();
                    break;
                case Constants.DELETE_FLASHCARD:
                    FlashCardService.DeleteFlashCard();
                    UpdateCache();
                    break;
                case Constants.MODIFY_FLASHCARD:
                    FlashCardService.UpdateFlashCard();
                    UpdateCache();
                    break;
                default:
                    break;
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadLine();
        }
    }

    public void RunStudySessionService()
    {
        while (true)
        {
            MenuHelper.PrintStudyManagementMenu();

            string? op = Console.ReadLine();
            Console.Clear();
            switch (op)
            {
                case Constants.BACK_TO_MAIN:
                    return;
                case Constants.SHOW_STUDYS:
                    StudySessionService.ShowAllStudySessions();
                    break;
                case Constants.START_NEW_STUDY:
                    StudySessionService.StartNewStudySession(StackDao.FindAll(), FlashCardDao.FindAll());
                    UpdateCache();
                    break;
                case Constants.DELETE_STUDY:
                    StudySessionService.DeleteStudySession();
                    UpdateCache();
                    break;
                default:
                    break;
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadLine();
        }
    }

    public void RunStudyReportService()
    {
        while (true)
        {
            MenuHelper.PrintStudyReportManagementMenu();

            string? op = Console.ReadLine();
            Console.Clear();
            switch (op)
            {
                case Constants.BACK_TO_MAIN:
                    return;
                case Constants.SHOW_STUDY_REPORT:
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