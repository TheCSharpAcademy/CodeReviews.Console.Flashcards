using Spectre.Console;

namespace FlashcardApp.Console.MessageLoggers;

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

        public static void DisplayMessage(string message)
        {
            string color = message.Equals("correct") ? "green": "red";
            AnsiConsole.MarkupLine($"The answer is [{color}]{message}![/]");
        }

        public static void DisplayFinalScoreMessage(string score)
        {
            AnsiConsole.Prompt(new TextPrompt<string>($"The final score is: [green]{score}[/]. Press any Key to continue").AllowEmpty());
        }

        public static void DisplayAverageScoreMessage(string averageScore)
        {
            AnsiConsole.Prompt(new TextPrompt<string>($"Average study session score is [skyblue1]{averageScore}[/]. Press any Key to continue").AllowEmpty());
        }
        public static void DisplaySimpleErrorMessage(string message)
        {
            AnsiConsole.Markup($"[red]{message}[/]");
        }
}