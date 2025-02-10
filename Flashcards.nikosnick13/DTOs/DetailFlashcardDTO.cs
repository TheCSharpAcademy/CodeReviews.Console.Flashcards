using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.nikosnick13.DTOs;

internal class DetailFlashcardDTO
{
    public int Id { get; set; }

    public string Question { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;

    public int StackId { get; set; }

    //TOD: πρεπτι να δω τι αλλο μεσα μπορω να μου βγαζι στην εκτιπωση
}
