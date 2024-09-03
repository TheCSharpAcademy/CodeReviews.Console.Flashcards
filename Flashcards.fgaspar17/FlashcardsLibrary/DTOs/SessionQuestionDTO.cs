namespace FlashcardsLibrary;
public class SessionQuestionDTO
{
    public int Id { get; set; }
    public required string Question { get; set; }
    public required string Answer { get; set; }
    public required string UserResponse { get; set; }
    public bool Result { get; set; }
}