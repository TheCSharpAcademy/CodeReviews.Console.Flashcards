namespace Flashcards.StanimalTheMan.DTOs;

internal class StudyPivotDto
{
    public StudyPivotDto(string stackName, List<int>monthlyValues)
    {
        StackName = stackName;
        MonthlyValues = monthlyValues;
    }

    public string StackName { get; set; }
    public List<int> MonthlyValues { get; set; }
}
