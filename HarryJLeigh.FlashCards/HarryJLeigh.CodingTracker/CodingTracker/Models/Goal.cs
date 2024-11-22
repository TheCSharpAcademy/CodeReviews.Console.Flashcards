namespace CodingTracker.Models;

public class Goal
{
    internal int Id;
    internal string StartDate;
    internal string DateToComplete;
    internal string Hours;

    internal DateTime ParsedStartDate => DateTime.Parse(StartDate);
    internal DateTime ParsedDateToComplete => DateTime.Parse(DateToComplete);
}