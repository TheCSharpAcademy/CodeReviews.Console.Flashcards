using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jollejonas.Flashcards.DTOs
{
    public class CardStackDetailsDTO
    {
        public string CardStackName { get; set; }
        public List<CardsDTO> Cards { get; set; }
    }
}
