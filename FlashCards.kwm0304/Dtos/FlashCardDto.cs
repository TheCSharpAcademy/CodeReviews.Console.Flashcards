namespace FlashCards.kwm0304.Dtos;

public class FlashCardDto
{
  public int FlashCardNumber { get; set; }
  public string? Question { get; set; }
  public string? Answer { get; set; }

  public FlashCardDto(int orderId, string question, string answer)
    {
        FlashCardNumber = orderId;
        Question = question;
        Answer = answer;
    }
}