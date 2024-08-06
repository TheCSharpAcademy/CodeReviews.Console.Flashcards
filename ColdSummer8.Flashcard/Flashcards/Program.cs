namespace Flashcards;
internal class Program
{
    static async Task Main(string[] args)
    {
        bool openApp = true;
        await View.Check(openApp);
    }
}
