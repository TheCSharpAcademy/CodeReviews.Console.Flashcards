namespace Flashcards;

using static System.Environment;

internal static class Logger
{
    static readonly string logFileName = "Flashcards.log";
    static readonly string loglevelError = "ERROR";
    static readonly string loglevelInfo = "INFO";

    public static void Error(Exception ex)
    {
        Error($"{ex.Message}{NewLine}{ex.StackTrace}");
    }

    public static void Error(string message)
    {
        WriteLog(loglevelError, message);
    }

    public static void Info(string message)
    {
        WriteLog(loglevelInfo, message);
    }

    private static void WriteLog(string level, string message)
    {
        var timestamp = DateTime.Now.ToString("u");
        using StreamWriter sw = File.AppendText(logFileName);
        sw.WriteLine($"{timestamp} {level.ToUpper()} {message}");
    }
}