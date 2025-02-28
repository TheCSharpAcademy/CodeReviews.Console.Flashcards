namespace vcesario.Flashcards;

public static class MainApplication
{
    public static void Run()
    {
        DataService.Initialize();

        Console.WriteLine("Hello, World!");
        Console.ReadLine();
    }
}