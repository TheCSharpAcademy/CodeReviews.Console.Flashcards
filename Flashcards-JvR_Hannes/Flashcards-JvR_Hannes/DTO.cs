namespace Flashcards_JvR_Hannes
{
    internal class Dto
    {
        public class FlashcardsDto
        {
            public int DisplayId { get; set; }
            private string _question;
            public string Question 
            {
                get => _question;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Question cannot be empty");
                    }
                    _question = value;
                }
            }
            private string _answer;
            public string Answer
            {
                get => _answer;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Answer cannot be empty");
                    }
                    _answer = value;
                }
            }
        }
        public class StackDto
        {
            public int StackId { get; set; }
            private string _stackName;
            public string StackName
            {
                get => _stackName;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Stack name cannot be empty");
                    }
                    _stackName = value;
                }
            }
            public List<FlashcardsDto> Flashcards { get; set; } = new List<FlashcardsDto>();
        }
        public class StudySessionDto
        {
            public int SessionId { get; set; }
            public int StackId { get; set; }
            public DateTime Date { get; set; }
            private int _score;
            public int Score
            {
                get => _score;
                set
                {
                    if (value < 0 || value > 100)
                    {
                        throw new ArgumentException("Score must be between 0 and 100.");
                    }
                    _score = value;
                }
            }
        }
    }
}
