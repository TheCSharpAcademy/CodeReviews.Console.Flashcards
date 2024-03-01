using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards;

public class Flashcard
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int StackId { get; set; }
}

public class FlashcardQuestionDto
{
    public string Question { get; set; }
}

public class FlashcardReviewDto
{
    public int DisplayId { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int StackId { get; set; }
}
//prostudovat DTOS v discordu (při nových indexech vytvořit nový list DTO a zpětné mapování bude probíhat na základě pořadí ID v daném listu. (hledej v discordu "Let's say you have 3 items in the db with indexes 10, 20 and 30" ))
