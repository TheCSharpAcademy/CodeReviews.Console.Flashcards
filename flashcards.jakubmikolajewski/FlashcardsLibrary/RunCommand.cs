namespace FlashcardsLibrary;

public abstract class RunCommand<T> where T : new ()
{
    static RunCommand() { }

    private static readonly T run = new T();

    public static T Run
    {
        get => run;
    }
}

