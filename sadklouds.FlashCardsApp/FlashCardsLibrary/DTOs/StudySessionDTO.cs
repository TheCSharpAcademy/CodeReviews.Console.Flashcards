using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardsLibrary.DTOs
{
    public class StudySessionDTO
    {
        public int Id { get; set; }
        public  DateTime Date { get; set; }
        public int Score { get; set; }
        public string StackName { get; set; }
    }
}
