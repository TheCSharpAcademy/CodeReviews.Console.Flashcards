using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardsApp.Models
{
    internal class Flashcard
    {
        public int FlashcardId { get; set; }
        public int StackId { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
