namespace Flashcards;


class Program
{
    static readonly int ERRORCODETABLEEXISTS = -2146232060;
    static async Task Main(string[] args)
    {
        await DataBaseManager.Start();
    }
}
