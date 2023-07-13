namespace FlashCards.Model
{
	internal class StudySession
	{
        public int StudySessionId { get; set; }
		public int StackId { get; set; }
		public DateTime StudyDate { get; set; }
        public int Score { get; set; }
    }
}
