namespace FlashCardsLibrary.Models
{
    public class StudySession
    {
        public string Name { get; set; }
        public Stack Stack { get; set; }
        public DateTime Date { get; set; }
        public int Answers { get; set; }
        public int Total { get; set; }
        public StudySession(string name,Stack stack,DateTime date,int answers) 
        {
            Name = name;
            Stack = stack;
            Date = date;
            Answers = answers;
            Total = FlashCardController.GetFlashCards(stack).Count;
        }
        public StudySession(string name, Stack stack, DateTime date, int answers,int total)
        {
            Name = name;
            Stack = stack;
            Date = date;
            Answers = answers;
            Total = total;
        }
    }
}
