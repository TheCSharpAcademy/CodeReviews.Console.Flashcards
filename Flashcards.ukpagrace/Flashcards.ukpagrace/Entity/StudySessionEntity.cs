namespace Flashcards.ukpagrace.Entity
{
    class StudySessionEntity
    {
        public int Id { get; set; }
        public int StackId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Score { get; set; }  
    }
}