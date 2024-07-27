using Flashcards.Database;
using Flashcards.Models;

namespace Flashcards.Handlers;

public class ViewStudyHistoryHandler(DbContext dbContext)
{
    private readonly DbContext db = dbContext;
    public const string MenuName = "View Study History";

    public async Task Handle()
    {
        while (true)
        {
            string[] menuOptions = ["Back to main menu", "View all study sessions"];
            var choice = UI.MenuSelection("View Study History Menu. Select an option below:", menuOptions);

            switch (choice)
            {
                case 0:
                    return;
                case 1:
                    await HandleViewAllStudySessions();
                    break;
            }
        }
    }

    private async Task HandleViewAllStudySessions()
    {
        var studySessions = await db.GetStudySessions();

        UI.DisplayStudySessions(studySessions);
        UI.ConfirmationMessage("");
    }
}