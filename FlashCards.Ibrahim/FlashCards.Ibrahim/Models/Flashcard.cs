using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Ibrahim.Models
{
    internal class Flashcard
    {
        public int Id { get; set; }
        public int Stacks_Id { get; set; }
        public string Front {  get; set; }
        public string Back { get; set; }

    }
}
