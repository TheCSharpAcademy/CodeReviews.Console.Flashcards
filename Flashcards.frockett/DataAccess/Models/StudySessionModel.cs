
namespace Library.Models;

public class StudySessionModel
{
    public int Id { get; set; }
    public int CardId { get; set; }
    public int StackId { get; set; }
    public DateTime SessionDateTime { get; set; }
    public bool Correct { get; set; }
    public int Score { get; set; }

    /* Note that the CardId currently is unusable. It would be nice to track which cards were right and wrong and how many times,
     * but currently this won't work because the session Id autoincrements. So for now, we can only get the Stack studied
     * and the total score. 
     * TODO either fix this or delete the correct BIT from the SQL table
    */
}
