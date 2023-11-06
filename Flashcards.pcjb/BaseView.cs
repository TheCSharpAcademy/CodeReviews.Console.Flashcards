namespace Flashcards;

abstract class BaseView
{
    private string? message;
    
    public abstract void Body();

    public void Show()
    {
        Header();
        Body();
    }

    private void Header()
    {
        Console.Clear();
        Console.WriteLine($"{new string('=', 12)} Flashcards {new string('=', 12)}");
        if (!string.IsNullOrEmpty(message))
        {
            Console.WriteLine(message);
            Console.WriteLine(new string('-', 40));
        }
    }

    public void SetMessage(string? msg)
    {
        message = msg;
    }
}