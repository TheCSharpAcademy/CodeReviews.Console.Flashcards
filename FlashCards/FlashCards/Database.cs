using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards
{
    public class Database
    {
        public class FlashCard
        {
            // The DTO?
            public string Id { get; set; }
            public string Challenge { get; set; }
            public string Answer { get; set; }
            public string Blah { get; set; }
        }

        public static FlashCard GetFlashCard()
        {
            // The method to set the DTO?
            FlashCard thisFlashCard = new()
            {
                Id = "1",
                Challenge = "The question",
                Answer = "The Answer",
                Blah = "Another thing"
            };

            return thisFlashCard;
        }

    }

    


}
