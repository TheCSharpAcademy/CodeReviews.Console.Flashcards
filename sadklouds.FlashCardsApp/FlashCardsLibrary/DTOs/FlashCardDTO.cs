using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardsLibrary.DTOs
{
    public class FlashCardDTO
    {
        public int Id { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
    }
}
