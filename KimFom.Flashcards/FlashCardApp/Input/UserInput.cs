namespace FlashCardApp.Input;

public class UserInput
{
    public string GetChoice()
    {
        return Console.ReadLine().Trim().ToLower();
    }

    public string GetInput()
    {
        return Console.ReadLine().Trim();
    }
}