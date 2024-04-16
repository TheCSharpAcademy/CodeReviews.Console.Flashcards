namespace DatabaseLibrary.Models;

public class Stack
{
  public int Stack_Id { get; set; }
  public string Name { get; set; }

  public Stack() { }

  public Stack(int id, string name)
  {
    Stack_Id = id;
    Name = name;
  }
}