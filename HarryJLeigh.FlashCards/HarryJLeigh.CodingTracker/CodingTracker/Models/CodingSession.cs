namespace CodingTracker.Models;

public class CodingSession
{
    internal int Id;
    internal string StartTime;
    internal string EndTime;
    internal string? Duration;
    
    public DateTime ParsedStartedTime => DateTime.Parse(StartTime);
    public DateTime ParsedEndTime => DateTime.Parse(EndTime);
}