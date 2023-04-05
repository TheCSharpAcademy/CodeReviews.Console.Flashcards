using FlashCardsLibrary.DTOs;
using FlashCardsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardsLibrary.Tools
{
    public static class CreateDTOHelper
    {

        public static List<FlashCardDTO> CreateFlashCardDTO(List<FlashCardModel> flashcards)
        {
            int id = 1;
            List<FlashCardDTO> output = new();
            if (flashcards != null)
            {
                foreach (FlashCardModel card in flashcards)
                {
                    
                    FlashCardDTO flashcardDTO = new FlashCardDTO { Id = id, Front = card.Front, Back = card.Back };
                    output.Add(flashcardDTO);
                    id++;
                }
            }
            else
            {
                Console.WriteLine("No records contained");
            }
            return output;
        }
    }
}
