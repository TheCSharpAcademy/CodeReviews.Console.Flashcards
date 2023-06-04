using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.JsPeanut
{
    class Flashcard
    {
        public int FlashcardId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public int? Rating { get; set; }

        public int StackId { get; set; }
    }
}
