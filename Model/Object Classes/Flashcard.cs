
class Flashcard(int stacks_Id, int id, string front, string back) : Data(id)
{
    public int StacksId => stacks_Id;
    public string Front => front;
    public string Back => back;
}