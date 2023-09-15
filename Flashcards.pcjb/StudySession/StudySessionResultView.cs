namespace Flashcards;

class StudySessionResultView : BaseView
{
    private readonly StudySessionController controller;
    private readonly Stack stack;
    private readonly StudySessionResult result;

    public StudySessionResultView(StudySessionController controller, Stack stack, StudySessionResult result)
    {
        this.controller = controller;
        this.stack = stack;
        this.result = result;
    }

    public override void Body()
    {
        Console.WriteLine($"Congratulations!");
        Console.WriteLine($"You completed the stack '{stack.Name}'.");
        Console.WriteLine($"Your Results:");
        Console.WriteLine($"Total Questions: {result.TotalQuestions}");
        Console.WriteLine($"Correct Answers: {result.CorrectAnswers}");
        Console.WriteLine($"Score: {result.ScorePercent}%");
        Console.WriteLine("Press enter to return to menu.");
        Console.ReadLine();
        controller.ShowMenu();
    }
}