using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Model
{
	internal class FlashcardDTO
	{
        public int Id { get; set; }
        public string FrontText { get; set; }
        public string BackText { get; set; }
    }
}
