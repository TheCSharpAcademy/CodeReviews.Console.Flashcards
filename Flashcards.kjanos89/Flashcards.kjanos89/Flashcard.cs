using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.kjanos89
{
    public class Flashcard
    {
        public int FlashcardId { get; set; }
        public int StackId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public Stack Stack { get; set; }
    }
}
