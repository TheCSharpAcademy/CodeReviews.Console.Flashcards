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
    public int StackId { get; set; }
}

public class FlashcardReviewDto
{
    public string Question { get; set; }
    public string Answer {get; set;}
    public int StackId { get; set;}
}
