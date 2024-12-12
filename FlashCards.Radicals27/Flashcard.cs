namespace flashcard_app
{
    public class FlashcardDTO
    {
        public required int FlashcardID { get; set; }
        public required string FrontText { get; set; }
        public required string BackText { get; set; }
    }

    public class Stack
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }

    public class StudySession
    {
        public required int StackID { get; set; }
        public required int Score { get; set; }
        public required int ScoreMax { get; set; }
        public required DateTime SessionDate { get; set; }
    }
}