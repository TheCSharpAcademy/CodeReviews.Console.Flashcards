namespace FlashCards.Cactus.DataModel
{
    public class StudySession
    {
        #region Constructors
        public StudySession() { }

        public StudySession(int id, int sId, DateTime date, int score)
        {
            Id = id;
            SId = sId;
            Date = date;
            Score = score;
        }

        #endregion Constructors

        #region Properties
        public int Id { get; set; }
        public int SId { get; set; }

        public DateTime Date { get; set; }

        public int Score { get; set; }

        #endregion Properties
    }
}
