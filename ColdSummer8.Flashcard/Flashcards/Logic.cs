using Spectre.Console;

namespace Flashcards
{
    public class Logic
    {
        public static void Do(int userInput, out bool openApp)
        {
            openApp = true;
            switch (userInput)
            {
                case 0:
                    Console.Clear();
                    Thread.Sleep(1000);
                    AnsiConsole.Write(new Markup("[red]Exiting...[/]\n"));
                    Environment.Exit(0);
                    break;
                case 1:
                    StackMethod.SelectStack();
                    break;
                case 2:
                    StackMethod.CreateStack();
                    break;
                case 3:
                    StackMethod.DeleteStack();
                    break;
                case 4:
                    FlashcardMethod.GetAllFlashcards(out bool flag);
                    break;
                case 5:
                    FlashcardMethod.CreateFlashcard();
                    break;
                case 6:
                    FlashcardMethod.DeleteFlashcard();
                    break;
                case 7:
                    FlashcardMethod.UpdateFlashcard();
                    break;
                case 8:
                    StudyMethod.StudySession();
                    break;
                case 9:
                    StudyMethod.ReportCard();
                    break;
            }
        }
    }
}
