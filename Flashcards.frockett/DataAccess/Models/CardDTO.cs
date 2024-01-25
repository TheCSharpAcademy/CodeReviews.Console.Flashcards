
namespace Library.Models;

public class CardDTO
{
    public string Question { get; set; }
    public string Answer { get; set; }

    public CardDTO(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }
}
