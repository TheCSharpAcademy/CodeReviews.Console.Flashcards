using CodingTracker.Helpers;

namespace CodingTracker.Services;

public static class StopwatchService
{
    private static DateTime StartTime { get; set; }
    private static DateTime EndTime { get; set; }
    
    public static DateTime StartStopwatch()
    {
        DateTime now = DateTime.Now;
        string formattedStartTime = now.ToString("yyyy-MM-dd HH:mm");
        return DateTime.ParseExact(formattedStartTime, "yyyy-MM-dd HH:mm", null);
    }
    
    public static DateTime StopStopwatch()
    {
        DateTime now = DateTime.Now;
        string formattedEndTime = now.ToString("yyyy-MM-dd HH:mm");
        return DateTime.ParseExact(formattedEndTime, "yyyy-MM-dd HH:mm", null);
      
    }

    public static string TotalDuration(DateTime startTime, DateTime endTime)
    {
        return Utilities.CalculateDuration(StartTime, EndTime);
    }
}