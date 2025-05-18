using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Models.DTO
{
    public class StudySessionDTO
    {
        public int Score { get; set; }
        public DateTime StudyDate { get; set; }
        public string StackName {  get; set; }
    }
}
