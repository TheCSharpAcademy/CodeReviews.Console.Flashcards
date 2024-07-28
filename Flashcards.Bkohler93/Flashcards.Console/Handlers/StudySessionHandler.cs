using Flashcards.Database;
using Models;

namespace Flashcards.Handlers;

public class StudySessionHandler(DbContext dbContext)
{
    public const string MenuName = "Study"; 
    private readonly DbContext db = dbContext;
    private readonly StudyGame studyGame = new();

    public async Task Handle()
    {
        var stacks = await db.GetStacksInfosAsync();
        var stackNames = stacks.Select(s => s.Name);
        while (true)
        {
            UI.Clear();
            string[] menuOptions = ["Back to main menu", ..stackNames];
            var choice = UI.MenuSelection("[green]Study[/] [blue]Menu[/]. Select a stack to study from below:", menuOptions);

            if (choice == 0)
            {
                return;
            } 
            
            else {
                var stack = await db.GetPlayStackById(stacks.ElementAt(choice-1).Id);
                var score = studyGame.Play(stack);
                var studySession = new CreateStudySessionDto{
                    StackId = stack.Id,
                    Score = score,
                    StudyTime = DateTime.Now,
                };

                await db.CreateNewStudySession(studySession);
            }
        }
    }
}
