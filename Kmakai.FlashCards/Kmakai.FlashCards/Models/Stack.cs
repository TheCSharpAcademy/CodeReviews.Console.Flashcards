using Kmakai.FlashCards.Controllers;

namespace Kmakai.FlashCards.Models;

public class Stack

{
    public int Id { get; set; }
    public string Name { get; set; }
    public Stack(string name)
    {
        Name = name;
    }
}
