using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardsLibrary.Models
{
    public class FlashCardModel
    {
        public int Id { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public int StackId { get; set; }
    }
}
