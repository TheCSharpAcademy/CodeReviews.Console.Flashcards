namespace FlashcardsLibrary;
public class SessionQuestionDto
{
    public int Id { get; set; }
    public required string Question { get; set; }
    public required string Answer { get; set; }
    public required string UserResponse { get; set; }
    public bool Result { get; set; }
}