using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Models
{
    public class StudySession
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime StudyDate { get; set; }
        public int StackId { get; set; }
    }
}
