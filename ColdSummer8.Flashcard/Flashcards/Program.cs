namespace Flashcards;
internal class Program
{
    static async Task Main()
    {
        bool openApp = true;
        await View.Check(openApp);
    }
}
