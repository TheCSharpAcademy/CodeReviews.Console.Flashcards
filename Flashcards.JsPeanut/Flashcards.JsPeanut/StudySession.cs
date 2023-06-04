using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.JsPeanut
{
    class StudySession
    {
        public int Id { get; set; }
        public string Rate { get; set; }

        public int RateId { get; set; }

        public int FlashcardId { get; set; }
    }
}
