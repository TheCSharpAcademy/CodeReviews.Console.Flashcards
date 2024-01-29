
namespace Library.Models;

public class CardDto
{
    public string Question { get; set; }
    public string Answer { get; set; }

    public CardDto(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }
}
