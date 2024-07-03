using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jollejonas.Flashcards.DTOs
{
    public class CardsDTO
    {
        public int Id { get; set; }
        public int PresentationId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
