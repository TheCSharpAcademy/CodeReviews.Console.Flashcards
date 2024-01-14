namespace FlashCards;

internal class StudySessions
{
    public int ID { get; set; }
    public string Subject { get; set;}
    public string FlashSubject { get; set;}
    public string FrontCard { get; set;}
    public string BackCard { get; set;}
    public DateTime Date { get; set;}
    public double GameScore { get; set;}
    public int GameAmount { get; set;}
}
