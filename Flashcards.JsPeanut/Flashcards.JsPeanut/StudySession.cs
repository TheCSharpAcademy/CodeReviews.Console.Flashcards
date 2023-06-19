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

        public int Hits { get; set; }

        public int Misses { get; set; }

        public int Score { get; set; }

        public int Month { get; set; }

        public int StackId { get; set; }
        public string StackName { get; set; }
    }
}
