using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCard.SheheryarRaza.Entities
{
    public class StudySession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime EndTime { get; set; } = DateTime.UtcNow;
        public int StackId { get; set; }
        public Stacks Stack { get; set; } = null!;
        public TimeSpan Duration => EndTime - StartTime;
        public int Score { get; set; } = 0;
        public int TotalQuestions { get; set; } = 0;

    }
}
