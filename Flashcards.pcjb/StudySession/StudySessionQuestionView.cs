namespace Flashcards;

class StudySessionQuestionView : BaseView
{
    private readonly StudySessionController controller;
    private readonly Stack stack;
    private readonly Flashcard card;
    private readonly int answeredQuestions;
    private readonly int totalQuestions;

    public StudySessionQuestionView(StudySessionController controller, Stack stack, Flashcard card, int answeredQuestions, int totalQuestions)
    {
        this.controller = controller;
        this.stack = stack;
        this.card = card;
        this.answeredQuestions = answeredQuestions;
        this.totalQuestions = totalQuestions;
    }

    public override void Body()
    {
        Console.WriteLine($"Study Session '{stack.Name}'");
        Console.WriteLine($"Card {answeredQuestions + 1} of {totalQuestions}");
        Console.WriteLine($"Question   : {card.Front}");
        Console.Write("Your Answer: ");
        var answer = Console.ReadLine();
        controller.CheckAnswer(card, answer);
    }
}