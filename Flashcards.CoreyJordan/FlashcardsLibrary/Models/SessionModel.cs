namespace FlashcardsLibrary.Models;
public class SessionModel
{
    public int Id { get; set; }
    public string Player { get; set; }
    public string Pack { get; set; }
    public int PackSize { get; set; }
    public DateTime Date { get; set; }
    public int Cycles { get; set; }
    public int CardsShown { get; set; }
    public int Correct { get; set; }
    public double Score
    {
        get
        {
            double average = Correct / CardsShown * 100;
            average -= 5 * Cycles;
            return average;
        }
    }
}
