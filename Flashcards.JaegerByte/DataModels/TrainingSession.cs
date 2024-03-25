namespace Flashcards.JaegerByte.DataModels
{
    internal class TrainingSession
    {
        public int SessionID { get; set; }
        public int StackID { get; set; }
        private string _startDate;
        public DateTime StartDate
        {
            get { return DateTime.Parse(_startDate); }
            set { _startDate = value.ToString(); }
        }
        private string _endDate;
        public DateTime EndDate
        {
            get { return DateTime.Parse(_endDate); }
            set { _endDate = value.ToString(); }
        }
        public TimeSpan Duration
        {
            get
            {
                return EndDate.TimeOfDay - StartDate.TimeOfDay;
            }
        }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public double Score
        {
            get
            {
                return (double)CorrectAnswers / ((double)CorrectAnswers+(double)WrongAnswers);
            }
        }

        public TrainingSession(int sessionID, int stackID, string startDate, string endDate, int correctAnswers, int wrongAnswers)
        {
            SessionID = sessionID;
            StackID = stackID;
            StartDate = DateTime.Parse(startDate);
            EndDate = DateTime.Parse(endDate);
            CorrectAnswers = correctAnswers;
            WrongAnswers = wrongAnswers;
        }

        public TrainingSession(int stackID, DateTime startDate, DateTime endDate, int correctAnswers, int wrongAnswers)
        {
            StackID = stackID;
            StartDate = startDate;
            EndDate = endDate;
            CorrectAnswers = correctAnswers;
            WrongAnswers = wrongAnswers;
        }
    }
}
