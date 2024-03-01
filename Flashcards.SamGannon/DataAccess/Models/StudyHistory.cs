using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class StudyHistory
    {
        public int Id { get; set; }
        public int StackId { get; set; }
        public string? StackName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Score { get; set; }
    }
}
