using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Study.Models.Domain
{
    public class flashcard
    {
        public int ID { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public string stack_name { get; set; }

    }
}
