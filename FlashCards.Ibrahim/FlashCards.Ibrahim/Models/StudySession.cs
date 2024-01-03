using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Ibrahim.Models
{
    internal class StudySession
    {
        public int Id { get; set; } 
        public int Stacks_Id { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }

    }
}
