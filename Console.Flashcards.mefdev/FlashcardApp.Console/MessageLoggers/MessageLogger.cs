using Spectre.Console;

namespace FlashcardApp.Console.MessageLoggers

{
	public static class MessageLogger
    {

        public static void DisplaySuccessMessage(string message)
        {
            AnsiConsole.MarkupLine($"[green]Success: {message}[/]");
        }

        public static void DisplayErrorMessage(string message)
        {
            AnsiConsole.MarkupLine($"[red]An error occurred: {message}[/]");
        }
        public static void DisplayNoticeMessage(string message)
        {
            AnsiConsole.MarkupLine($"[yellow]Notice: {message}[/]");
        }
        public static void DisplayQuitMessage()
        {
            AnsiConsole.MarkupLine($"[yellow]Quitting the application: Thank you for using our Flashcard APP[/]");
        }

    }
	
}

