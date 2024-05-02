using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace AdityaFlashCards.Database.Models
{
    public class FlashCardDTOStackView
    {
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public int PositionInStack { get; set; }
    }
}
