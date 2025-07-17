using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FlashCard.SheheryarRaza.DataBase;
using FlashCard.SheheryarRaza.Entities;

namespace FlashCard.SheheryarRaza.Managers
{
    public class StudySessionManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly StackManager _stackManager;

        public StudySessionManager(IServiceProvider serviceProvider, StackManager stackManager)
        {
            _serviceProvider = serviceProvider;
            _stackManager = stackManager;
        }

        public void StartStudySession()
        {
            ConsoleHelper.ClearConsole();
            ConsoleHelper.DisplayMessage("--- Start Study Session ---", ConsoleColor.Green);
            _stackManager.ViewAllStacks();
            string stackName = ConsoleHelper.GetStringInput("Enter the Name of the stack you want to study: ");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                try
                {
                    var stack = context.Stacks.Include(s => s.FlashCards)
                                        .FirstOrDefault(s => s.Name.ToLower() == stackName.ToLower());

                    if (stack == null)
                    {
                        ConsoleHelper.DisplayMessage($"Stack with name '{stackName}' not found.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    var flashcardsToStudy = stack.FlashCards.OrderBy(fc => Guid.NewGuid()).ToList();
                    if (!flashcardsToStudy.Any())
                    {
                        ConsoleHelper.DisplayMessage("No flashcards in this stack to study.", ConsoleColor.Yellow);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    ConsoleHelper.DisplayMessage($"Starting study session for stack: '{stack.Name}'", ConsoleColor.Cyan);
                    ConsoleHelper.DisplayMessage("Press Enter to reveal answer, type your answer and press Enter to check.", ConsoleColor.Yellow);
                    ConsoleHelper.PressAnyKeyToContinue();

                    var sessionStartTime = DateTime.UtcNow;
                    int correctAnswers = 0;
                    int totalQuestions = flashcardsToStudy.Count;

                    foreach (var flashcard in flashcardsToStudy)
                    {
                        ConsoleHelper.ClearConsole();
                        ConsoleHelper.DisplayMessage($"Question: {flashcard.Question}", ConsoleColor.White);
                        ConsoleHelper.GetStringInput("Press Enter to reveal answer...");

                        ConsoleHelper.DisplayMessage($"Answer: {flashcard.Answer}", ConsoleColor.Green);
                        string userAnswer = ConsoleHelper.GetStringInput("Your answer (type and press Enter): ");

                        if (userAnswer.Trim().Equals(flashcard.Answer.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            ConsoleHelper.DisplayMessage("Correct!", ConsoleColor.Green);
                            correctAnswers++;
                        }
                        else
                        {
                            ConsoleHelper.DisplayMessage("Incorrect.", ConsoleColor.Red);
                            ConsoleHelper.DisplayMessage($"The correct answer was: {flashcard.Answer}", ConsoleColor.Yellow);
                        }
                        ConsoleHelper.PressAnyKeyToContinue();
                    }

                    var sessionEndTime = DateTime.UtcNow;
                    var studySession = new StudySession
                    {
                        StackId = stack.Id,
                        StartTime = sessionStartTime,
                        EndTime = sessionEndTime,
                        Score = correctAnswers,
                        TotalQuestions = totalQuestions
                    };

                    context.StudySessions.Add(studySession);
                    context.SaveChanges();

                    ConsoleHelper.ClearConsole();
                    ConsoleHelper.DisplayMessage("--- Study Session Complete! ---", ConsoleColor.Cyan);
                    ConsoleHelper.DisplayMessage($"Stack: {stack.Name}", ConsoleColor.White);
                    ConsoleHelper.DisplayMessage($"Total Questions: {totalQuestions}", ConsoleColor.White);
                    ConsoleHelper.DisplayMessage($"Correct Answers: {correctAnswers}", ConsoleColor.Green);
                    ConsoleHelper.DisplayMessage($"Score: {correctAnswers}/{totalQuestions}", ConsoleColor.Green);
                    ConsoleHelper.DisplayMessage($"Duration: {studySession.Duration.TotalSeconds:F2} seconds", ConsoleColor.White);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayMessage($"Error during study session: {ex.Message}", ConsoleColor.Red);
                }
            }
            ConsoleHelper.PressAnyKeyToContinue();
        }

        public void ViewStudySessions()
        {
            ConsoleHelper.ClearConsole();
            ConsoleHelper.DisplayMessage("--- All Study Sessions ---", ConsoleColor.Green);
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                try
                {
                    var sessions = context.StudySessions.Include(ss => ss.Stack)
                                            .OrderByDescending(ss => ss.StartTime)
                                            .ToList();

                    if (!sessions.Any())
                    {
                        ConsoleHelper.DisplayMessage("No study sessions found.", ConsoleColor.Yellow);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    Console.WriteLine("ID\tStack Name\tDate\t\t\tScore\tDuration (s)");
                    Console.WriteLine("--\t----------\t----\t\t\t-----\t------------");
                    foreach (var session in sessions)
                    {
                        Console.WriteLine($"{session.Id}\t{session.Stack.Name}\t\t{session.StartTime.ToLocalTime():yyyy-MM-dd HH:mm}\t{session.Score}/{session.TotalQuestions}\t{session.Duration.TotalSeconds:F2}");
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayMessage($"Error viewing study sessions: {ex.Message}", ConsoleColor.Red);
                }
            }
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }
}
