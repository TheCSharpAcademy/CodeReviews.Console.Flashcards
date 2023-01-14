using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lonchanick9427.FlashCard;

public class Deck
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Deck() { }
    public Deck(int id, string nombre, string desciption) 
    {
        Id = id;
        Name = nombre;
        Description = desciption;
    }
}
