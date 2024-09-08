using System;

namespace Flash_Cards.Lawang;

public class FlashCard
{
    public int Id { get; set; }
    public string Front { get; set; } = "";
    public string Back { get; set; } = "";
    public int ForeignKey { get; set; }
}
