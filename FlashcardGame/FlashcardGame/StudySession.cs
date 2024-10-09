namespace FlashcardGame
{
    internal class StudySession
    {
        public int StudySession_Id { get; set; }
        public int Right_Answers { get; set; }
        public int Bad_Answers { get; set; }
        public int Stack_Id { get; set; }
        public DateTime Study_Date { get; set; }

        public StudySession()
        {

        }

        public override string ToString()
        {
            return string.Format("{0,-5} | {1,-10} | {2,-10} | {3,-20}", StudySession_Id, Right_Answers, Bad_Answers, Study_Date.ToString("yyyy-MM-dd HH:mm"));
        }
    }
}
