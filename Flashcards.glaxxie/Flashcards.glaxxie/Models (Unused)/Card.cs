using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.glaxxie.Models;

internal class Card
{
    internal int Id { get; set; }
    internal int StackId { get; set; }
    internal string Front { get; set; }
    internal string Back { get; set; }

    internal Card(int id, int stackId, string front, string back)
    {
        Id = id;
        StackId = stackId;
        Front = front;
        Back = back;
    }

    internal Card(string front, string back)
    {
        Front = front;
        Back = back;
    }
}
