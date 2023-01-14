using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lonchanick9427.FlashCard;

public class FlashCard
{
    public int Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public int Fk { get; set; }
    public FlashCard() { }
    public FlashCard(int id, string front, string back, int fk) 
    { 
        Id = id;
        Front = front;
        Back = back;
        Fk = fk;
    }
}
