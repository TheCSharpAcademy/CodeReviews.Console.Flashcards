namespace Flashcards.StanimalTheMan.DTOs;

internal class StudyPivotDTO
{
    public StudyPivotDTO(string stackName, List<int>monthlyValues)
    {
        StackName = stackName;
        MonthlyValues = monthlyValues;
    }

    public string StackName { get; set; }
    public List<int> MonthlyValues { get; set; }
}
