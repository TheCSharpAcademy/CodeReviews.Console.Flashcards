namespace FlashCards
{
    internal class StudySession
    {
        public string StackName { get; set; } = string.Empty;
        public int StackId { get; set; }
        public DateTime SessionDate { get; set; }
        public int Score { get; set; }
    }
}