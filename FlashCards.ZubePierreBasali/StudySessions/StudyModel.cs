namespace FlashCards.StudySessions
{
    internal interface IStudy
    {
        int? QuestionCount { get; set; }
    }
    internal class StudyModel : IStudy
    {
        public int? QuestionCount { get; set; }
        public int Score { get; set; }
        public string? Stack { get; set; }
        public string? QuestionMode { get; set; }
        public string? Date {  get; set; }
        public string? Time {  get; set; }
    }

    internal class StudyOptions : IStudy
    {
        public int? QuestionCount { get; set; }
        public string QuestionMode { get; set; }
        public string TimerOnOff { get; set; }
    }

    internal class StudyMonthly : IStudy
    {
        public int? QuestionCount {  get; set; }
        public string Month {  get; set; }
    }
}
