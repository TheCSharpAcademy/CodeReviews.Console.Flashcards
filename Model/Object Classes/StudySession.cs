
class StudySession(int stacks_Id, int id, string date, int score) : Data(id)
{
    public int Stacks_Id => stacks_Id;
    public string Date => date;
    public int Score => score;
}