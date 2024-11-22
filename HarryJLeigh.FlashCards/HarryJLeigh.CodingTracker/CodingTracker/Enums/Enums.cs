namespace CodingTracker;

internal enum Enums
{
    ViewAllSessions,
    ViewWithFilter,
    Insert,
    Update,
    Delete,
    Report,
    Stopwatch,
    Goal,
    Exit
}

internal enum UpdateSessionOptions
{
    StartTime,
    EndTime,
    Both
}

internal enum UpdateGoalOptions
{
    EndTime,
    Hours,
    Both
}

public enum FilterOptions
{
    Days,
    Weeks,
    Years
}

internal enum ReportOptions
{
    TotalSessions,
    AverageHours,
}

internal enum GoalOptions
{
    SetGoal,
    ShowGoals,
    UpdateGoal,
    CheckGoalStatus,
    DeleteGoal,
    DeleteAllGoals,
    Exit
}