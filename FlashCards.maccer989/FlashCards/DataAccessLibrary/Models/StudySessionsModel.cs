namespace DataAccessLibrary.Models
{
    public  class StudySessionsModel
    {
        public int id { get; set; }
        public int StackName { get; set; }
        public DateTime StudySessionDate { get; set; }
        public int TotalAnswerCorrect { get; set; }
        
    }
}
