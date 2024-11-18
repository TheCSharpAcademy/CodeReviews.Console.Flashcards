
namespace FlashCards.Models;

internal class Card
{
    public int FK_stack_id { get; set; }
    public int cardnumber { get; set; }
    public string front { get; set; }
    public string back { get; set; }
}

