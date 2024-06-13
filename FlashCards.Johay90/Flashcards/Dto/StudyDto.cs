public class StudyDto
{
    public int Id { get; set; }
    public string StackName { get; set; }
    public DateTime Date { get; set; }
    public double PercentageScore { get; set; }

    public override string ToString()
    {
        return $"{Id, -5} {StackName, -25} {Date, -15:d/MM/yyyy} {PercentageScore}%";
    }
}