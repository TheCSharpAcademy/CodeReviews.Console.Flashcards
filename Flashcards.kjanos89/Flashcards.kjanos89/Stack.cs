using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.kjanos89
{
    public class Stack
    {
        public int StackId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Flashcard> Flashcards { get; set; }
    }
}
