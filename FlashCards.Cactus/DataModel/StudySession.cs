namespace FlashCards.Cactus.DataModel
{
    public class StudySession
    {
        #region Constructors
        public StudySession() { }

        public StudySession(int id, string stackName, TimeSpan time, int score)
        {
            Id = id;
            StackName = stackName;
            Time = time;
            Score = score;
        }

        #endregion Constructors

        #region Properties
        public int Id { get; set; }
        public string StackName { get; set; }

        public TimeSpan Time { get; set; }

        public int Score { get; set; }

        #endregion Properties
    }
}
